using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Realt.Parser.Model;

namespace Realt.Parser
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

        public async Task<bool> AddRangeAsync(IEnumerable<Property> items, int operationId)
        {
            var result = true;
            foreach (var repository in _repositories)
            {
                try
                {
                    await repository.AddRangeAsync(items, operationId);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Could not process {repository.GetHashCode()} / {operationId}", ex);
                    result = false;
                }
            }

            return result;
        }
    }
}
