﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Realt.Parser.Model;

namespace Realt.Parser.DataAccess
{
    public class AggregatedRepository : IRepository
    {
        private readonly IRepository[] _repositories;
        private readonly ILogger<AggregatedRepository> _logger;

        public AggregatedRepository(IRepository[] repositories, ILogger<AggregatedRepository> logger)
        {
            _repositories = repositories;
            _logger = logger;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Property> items, string scanId, DateTime scanned)
        {
            var result = true;
            foreach (var repository in _repositories)
            {
                try
                {
                    await repository.AddRangeAsync(items, scanId, scanned);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Could not add range {repository.GetType()} / {scanId}", ex);
                    result = false;
                }
            }

            return result;
        }

        public async Task ClearAsync(string scanId)
        {
            foreach (var repository in _repositories)
            {
                try
                {
                    await repository.ClearAsync(scanId);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Could not clear {repository.GetType()} / {scanId}", ex);
                }
            }
        }
    }
}