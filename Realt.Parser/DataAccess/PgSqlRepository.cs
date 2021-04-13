using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Realt.Parser.Model;
using Microsoft.Extensions.Logging;
using System.Linq;
using Dapper;

namespace Realt.Parser.DataAccess
{
    public class PgSqlRepository : IRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<PgSqlRepository> _logger;
        private readonly string _table;
        private const int RetryCount = 3;

        private const string DefaultTable = "history";

        private const string InsertSql = @"
INSERT INTO {0}(
	    id, scan_id, source, scanned, room_total, room_separate, year, year_from, year_to, square_total, square_living, square_kitchen, floor, floor_total, price_usd, price_byn, type, balcony, district, address, created, error)
VALUES (@Id, @ScanId, @Source, @Scanned, @RoomTotal, @RoomSeparate, @Year, @YearFrom, @YearTo, @SquareTotal, @SquareLiving, @SquareKitchen, @Floor, @FloorTotal, @PriceUsd, @PriceByn, @Type, @Balcony, @District, @Address, @Created, @Error);";

        public PgSqlRepository(string connectionString, ILogger<PgSqlRepository> logger, string table = DefaultTable)
        {
            _connectionString = connectionString;
            _logger = logger;
            _table = table;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Property> items, string scanId, DateTime scanned)
        {
            try
            {
                
                var result = true;
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var historyItems = items.Select(item => History.Create(item, scanId, scanned));

                    for (var i = 0; i < RetryCount; i++)
                    {
                        try
                        {
                            await conn.ExecuteAsync(string.Format(InsertSql, _table), historyItems);
                            break;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Could not save entities DB for {scanId}, attempt {i}");
                            result = false;
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not save data to DB for {scanId}");
                return false;
            }
        }

        public async Task ClearAsync(string scanId, int source)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    await conn.ExecuteAsync($"DELETE FROM {_table} WHERE scan_id = @ScanId AND source = @Source", new { ScanId = scanId, Source = source });
                    _logger.LogInformation($"Cleaned up data for {_table} / {scanId} / {source}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not clean up data for {_table} / {scanId} / {source}");
            }
        }
    }
}
