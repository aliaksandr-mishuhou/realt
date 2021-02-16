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

        private const int RetryCount = 3;

        private const string InsertSql = @"
INSERT INTO history(
	    id, scan_id, scanned, room_total, room_separate, year, square_total, square_living, square_kitchen, floor, floor_total, price_usd, price_byn, type, balcony, district, address, created, error)
VALUES (@Id, @ScanId, @Scanned, @RoomTotal, @RoomSeparate, @Year, @SquareTotal, @SquareLiving, @SquareKitchen, @Floor, @FloorTotal, @PriceUsd, @PriceByn, @Type, @Balcony, @District, @Address, @Created, @Error);";

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
                    var historyItems = items.Select(item => History.Create(item, scanId, scanned));

                    for (var i = 0; i < RetryCount; i++)
                    {
                        try
                        {
                            await conn.ExecuteAsync(InsertSql, historyItems);
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

        public async Task ClearAsync(string scanId)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    await conn.ExecuteAsync("DELETE FROM history WHERE scan_id = @ScanId", new { ScanId = scanId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not clean up data for {scanId}");
            }
        }
    }
}
