using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Realt.Parser.DataAccess;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public class Runner : IRunner
    {
        private readonly IParserV1 _parserV1;
        private readonly IParserV2 _parserV2;
        private readonly IRepository _repository;
        private readonly ILogger<Runner> _logger;

        //private readonly int[] YearRanges = { 1900, 1950, 1960, 1970, 1980, 1990, 2000, 2005, 2010, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022 };
        //private readonly int[] RoomRanges = { 1, 2, 3, 4, 5 };
        //private const int MaxRooms = 6;

        private readonly List<long> _foundIds = new List<long>();

        public Runner(IParserV1 parserV1, IParserV2 parserV2, IRepository repository, ILogger<Runner> logger)
        {
            _parserV1 = parserV1;
            _parserV2 = parserV2;
            _repository = repository;
            _logger = logger;
        }

        #region v1

        public async Task RunV1Async()
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _logger.LogInformation("Starting...");
                // load general info (total & token)
                var info = await _parserV1.GetInfoAsync();
                _logger.LogInformation($"Total: {info.Total}, Pages: {info.TotalPages}, Token: {info.Token}");

                var scanId = DateTime.UtcNow.ToString("yyyy-MM-dd");
                await _repository.ClearAsync(scanId, _parserV1.Source);

                for (var i = 0; i <= info.TotalPages; i++)
                {
                    var items = await _parserV1.ReadPageAsync(info.Token, i);
                    var scanned = DateTime.UtcNow;

                    var found = items.Count();

                    await _repository.AddRangeAsync(items, scanId, scanned);
                    _logger.LogInformation($"Page {i}: {found}");

                    if (found == 0)
                    {
                        _logger.LogInformation($"Page {i}: exit due to no items");
                        break;
                    }
                }

                sw.Stop();
                _logger.LogInformation($"Completed. Elapsed: {sw.Elapsed}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatal error");
            }
        }

        #endregion

        #region v2

        public async Task RunV2Async()
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _logger.LogInformation("Starting...");
                _foundIds.Clear();

                var scanId = DateTime.UtcNow.ToString("yyyy-MM-dd");
                await _repository.ClearAsync(scanId, _parserV2.Source);

                foreach (var search in _parserV2.GetSearchSequence())
                {
                    await RunV2ChunkAsync(scanId, search);
                }

                sw.Stop();
                _logger.LogInformation($"Completed. Elapsed: {sw.Elapsed}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatal error");
            }
        }

        private async Task RunV2ChunkAsync(string scanId, Search search)
        {
            _logger.LogInformation($"Starting search [{search}]");

            // load general info (total & token)
            var info = await _parserV2.GetInfoAsync(search);

            _logger.LogInformation($"Found: total {info.Total}, pages: {info.TotalPages}, token: {info.Token}");

            var totalFound = 0;
            var totalSaved = 0;

            search.Token = info.Token;

            // load pages
            for (var i = 0; i <= info.TotalPages; i++)
            {
                var sw = new Stopwatch();

                sw.Start();
                var items = await _parserV2.ReadPageAsync(search, i);
                sw.Stop();
                var parsingTs = sw.Elapsed;

                var scanned = DateTime.UtcNow;

                var found = items.Count();
                totalFound += found;

                _logger.LogDebug($"Page {i}: total = {found}");

                if (found == 0)
                {
                    _logger.LogWarning($"Page {i}: exit due to no items");
                    break;
                }

                // check duplicates #1
                var selfDuplicates = items.GroupBy(i => i.Id)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key);
                if (selfDuplicates.Any())
                {
                    _logger.LogWarning($"Page {i}: found {selfDuplicates.Count()} self duplicates");
                    items = items.Where(i => !selfDuplicates.Contains(i.Id));
                }

                // check duplicates #2
                var prevDuplicates = items.Select(i => i.Id).Intersect(_foundIds);
                if (prevDuplicates.Any())
                {
                    _logger.LogWarning($"Page {i}: found {prevDuplicates.Count()} prev duplicates");
                    items = items.Where(i => !prevDuplicates.Contains(i.Id));
                }

                // set year range properties
                var itemsForSave = items.ToArray();
                foreach (var item in itemsForSave)
                {
                    item.YearFrom = search.YearFrom;
                    item.YearTo = search.YearTo;
                }

                sw.Restart();
                var saved = await _repository.AddRangeAsync(itemsForSave, scanId, scanned);
                sw.Stop();
                var writingTs = sw.Elapsed;

                _foundIds.AddRange(itemsForSave.Select(i => i.Id));
                totalSaved += saved;

                _logger.LogInformation($"Page {i} saved: found = {found}, saved = {saved}, parsing = {parsingTs}, writing = {writingTs} (progress = {_foundIds.Count})");
            }

            _logger.LogInformation($"Completed search [{search}]: expected = {info.Total}, found = {totalFound}, saved = {totalSaved} (progress = {_foundIds.Count})");
        }

        #endregion
    }
}
