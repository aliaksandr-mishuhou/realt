using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Realt.Parser.Core.Model;
using Microsoft.Extensions.Logging;
using System.Linq;
using Dapper;
using Newtonsoft.Json;
using Realt.Parser.Core;

namespace Realt.Parser.Repository.Db
{
    public class PgSqlRepository : IRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<PgSqlRepository> _logger;
        private readonly string _table;
        private const int RetryCount = 1;

        private const string DefaultTable = "history";

        private const string InsertSql = @"
INSERT INTO {0}(
	    id, scan_id, source, scanned, room_total, room_separate, year, year_from, year_to, square_total, square_living, square_kitchen, floor, floor_total, price_usd, price_byn, type, balcony, district, address, created, error)
VALUES (@Id, @ScanId, @Source, @Scanned, @RoomTotal, @RoomSeparate, @Year, @YearFrom, @YearTo, @SquareTotal, @SquareLiving, @SquareKitchen, @Floor, @FloorTotal, @PriceUsd, @PriceByn, @Type, @Balcony, @District, @Address, @Created, @Error);";

        private const string DeleteSql = "DELETE FROM {0} WHERE scan_id = @ScanId AND source = @Source";

        public PgSqlRepository(string connectionString, ILogger<PgSqlRepository> logger, string table = DefaultTable)
        {
            _connectionString = connectionString;
            _logger = logger;
            _table = table;
        }

        public async Task<int> AddRangeAsync(IEnumerable<Property> items, string scanId, DateTime scanned, int source)
        {
            var result = 0;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var historyItems = items.Select(item => Create(item, scanId, scanned, source));

                    for (var i = 0; i < RetryCount; i++)
                    {
                        try
                        {
                            var saved = await conn.ExecuteAsync(string.Format(InsertSql, _table), historyItems);
                            result += saved;
                            break;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Could not save entities DB for {scanId}, attempt {i}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not save data to DB for {scanId}");
            }

            return result;
        }

        public async Task ClearAsync(string scanId, int source)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var deleteSql = string.Format(DeleteSql, _table);
                    await conn.ExecuteAsync(deleteSql, new { ScanId = scanId, Source = source });
                    _logger.LogInformation($"Cleaned up data for {_table} / {scanId} / {source}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not clean up data for {_table} / {scanId} / {source}");
            }
        }

        public static History Create(Property property, string scanId, DateTime scanned, int source)
        {
            var propertyHistory = JsonConvert.DeserializeObject<History>(JsonConvert.SerializeObject(property));
            propertyHistory.ScanId = scanId;
            propertyHistory.Source = source;
            propertyHistory.Scanned = scanned;
            return propertyHistory;
        }
    }
}
