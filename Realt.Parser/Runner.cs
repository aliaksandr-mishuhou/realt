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

        private readonly int[] YearRanges = { 1900, 1950, 1960, 1970, 1980, 1990, 2000, 2005, 2010, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022 };
        private readonly int[] RoomRanges = { 1, 2, 3, 4, 5 };
        private const int MaxRooms = 6;

        private readonly List<long> _ids = new List<long>();

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
                _ids.Clear();

                var scanId = DateTime.UtcNow.ToString("yyyy-MM-dd");
                await _repository.ClearAsync(scanId, _parserV2.Source);

                var search = new Search();

                for (var y = 0; y < YearRanges.Length; y++)
                {
                    search.YearFrom = YearRanges[y];
                    search.YearTo = (y < YearRanges.Length - 1) ? (int?) YearRanges[y + 1] - 1 : null;

                    for (var r = 0; r < RoomRanges.Length; r++)
                    {
                        var rooms = RoomRanges[r];
                        if (r < RoomRanges.Length - 1)
                        {
                            search.Rooms = new int[] { rooms };
                        }
                        else
                        {
                            search.Rooms = Enumerable.Range(rooms, MaxRooms - rooms + 1).ToArray();
                        }

                        _logger.LogDebug($"Starting search [{search}]");
                        await RunV2ChunkAsync(scanId, search);
                        _logger.LogInformation($"Completed search [{search}]");
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

        private async Task RunV2ChunkAsync(string scanId, Search search)
        {
            // load general info (total & token)
            var info = await _parserV2.GetInfoAsync(search);
            _logger.LogDebug($"Total: {info.Total}, Pages: {info.TotalPages}, Token: {info.Token}");
            search.Token = info.Token;

            // load pages
            for (var i = 0; i <= info.TotalPages; i++)
            {
                var items = await _parserV2.ReadPageAsync(search, i);
                var scanned = DateTime.UtcNow;

                var found = items.Count();

                _logger.LogDebug($"Page {i}: {found}");

                if (found == 0)
                {
                    _logger.LogWarning($"Page {i}: exit due to no items");
                    break;
                }

                // check duplicates
                var duplicates = items.Select(i => i.Id).Intersect(_ids);
                if (duplicates.Any())
                {
                    _logger.LogWarning($"Page {i}: found {duplicates.Count()} duplicates");
                    items = items.Where(i => !duplicates.Contains(i.Id));
                }

                // set year range properties
                var extended = items.ToArray();
                foreach (var item in extended)
                {
                    item.YearFrom = search.YearFrom;
                    item.YearTo = search.YearTo;
                }

                await _repository.AddRangeAsync(extended, scanId, scanned);
            }
        }

        #endregion
    }
}
