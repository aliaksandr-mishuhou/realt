using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Realt.Parser.Core;
using Realt.Parser.Core.Model;

namespace Realt.Parser
{
    public class Runner : IRunner
    {
        private readonly IParser _parser;
        private readonly IRepository _repository;
        private readonly ILogger<Runner> _logger;

        private readonly List<long> _foundIds = new List<long>();

        public Runner(IParser parser, IRepository repository, ILogger<Runner> logger)
        {
            _parser = parser;
            _repository = repository;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            try
            {
                var sw = new Stopwatch();
                sw.Start();
                _logger.LogInformation("Starting...");
                _foundIds.Clear();

                var scanId = DateTime.UtcNow.ToString("yyyy-MM-dd");
                await _repository.ClearAsync(scanId, _parser.Source);

                foreach (var search in _parser.GetSearchSequence())
                {
                    await RunChunkAsync(scanId, search);
                }

                sw.Stop();
                _logger.LogInformation($"Completed. Elapsed: {sw.Elapsed}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatal error");
            }
        }

        private async Task RunChunkAsync(string scanId, Search search)
        {
            _logger.LogInformation($"Starting search [{search}]");

            // load general info (total & token)
            var info = await _parser.GetInfoAsync(search);

            _logger.LogInformation($"Found: total {info.Total}, pages: {info.TotalPages}, token: {info.Token}");

            var totalFound = 0;
            var totalSaved = 0;

            search.Token = info.Token;

            // load pages
            for (var i = 0; i <= info.TotalPages; i++)
            {
                var sw = new Stopwatch();

                sw.Start();
                var items = await _parser.ReadPageAsync(search, i);
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
                var saved = await _repository.AddRangeAsync(itemsForSave, scanId, scanned, _parser.Source);
                sw.Stop();
                var writingTs = sw.Elapsed;

                _foundIds.AddRange(itemsForSave.Select(i => i.Id));
                totalSaved += saved;

                _logger.LogInformation($"Page {i} saved: found = {found}, saved = {saved}, parsing = {parsingTs}, writing = {writingTs} (progress = {_foundIds.Count})");
            }

            _logger.LogInformation($"Completed search [{search}]: expected = {info.Total}, found = {totalFound}, saved = {totalSaved} (progress = {_foundIds.Count})");
        }
    }
}
