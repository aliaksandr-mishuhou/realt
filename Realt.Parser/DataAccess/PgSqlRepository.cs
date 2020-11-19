using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Realt.Parser.Model;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Realt.Parser.DataAccess
{
    public class PgSqlRepository : IRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<PgSqlRepository> _logger;

        public PgSqlRepository(string connectionString, ILogger<PgSqlRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Property> items, string scanId, DateTime scanned)
        {
            try
            {
                var result = true;
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var historyItems = items.Select(item => HistoryProperty.Create(item, scanId, scanned));
                    foreach (var historyItem in historyItems)
                    {
                        try
                        {
                            await conn.InsertAsync(historyItem);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Could not save entity {historyItem.Id} to DB for {scanId}", ex);
                            result = false;
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not save data to DB for {scanId}", ex);
                return false;
            }
        }

        public async Task ClearAsync(string scanId)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    // TODO: cleanup
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not clean up data for {scanId}", ex);
            }
        }
    }
}
