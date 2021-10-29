using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Realt.Parser.Core;
using Realt.Parser.Core.Model;
using Realt.Parser.Repository.Rest.Dto;

namespace Realt.Parser.Repository.Rest
{
    public class RestRepository : IRepository
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _basepath;
        private readonly ILogger<RestRepository> _logger;

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        public RestRepository(string basepath, ILogger<RestRepository> logger)
        {
            _basepath = basepath;
            _logger = logger;
        }


        public async Task<int> AddRangeAsync(IEnumerable<Property> items, string scanId, DateTime scanned, int source)
        {
            var url = $"{_basepath}/history";

            try
            {
                _logger.LogDebug($"Adding batch scanId/source = {scanId}/{source}, items = {items.Count()} / {url}");

                var request = new AddRangeRequest
                {
                    Source = source,
                    ScanId = scanId,
                    Scanned = scanned,
                    Items = items.ToArray()
                };

                var requestJson = JsonConvert.SerializeObject(request, jsonSerializerSettings);
                var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var httpResponse = await _httpClient.PostAsync(url, httpContent);
                var resultJson = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(
                        string.Format("Error response: code = {0}, reason = [{1}]", httpResponse.StatusCode, resultJson));
                }

                var response = JsonConvert.DeserializeObject<AddRangeResponse>(resultJson);
                _logger.LogInformation($"Added batch scanId/source = {scanId}/{source}, items = {items.Count()}, added = {response.Added}");
                return response.Added;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to add batch scanId/source = {scanId}/{source} / {url}");
                throw;
            }
        }

        public async Task ClearAsync(string scanId, int source)
        {
            //var url = $"{_basepath}/history";
            var url = $"{_basepath}/history/{source}/{scanId}";

            try
            {
                _logger.LogDebug($"Removing scanId/source = {scanId}/{source} / {url}");

                //var request = new ClearRequest
                //{
                //    ScanId = scanId,
                //    Source = source
                //};

                //var searchJson = JsonConvert.SerializeObject(request);
                //var httpContent = new StringContent(searchJson, Encoding.UTF8, "application/json");

                var httpResponse = await _httpClient.DeleteAsync(url);
                var resultJson = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    throw new Exception(
                        string.Format("Error response: code = {0}, reason = [{1}]", httpResponse.StatusCode, resultJson));
                }

                var response = JsonConvert.DeserializeObject<ClearResponse>(resultJson);
                _logger.LogInformation($"Removed scanId/source = {scanId}/{source}, removed = {response.Removed}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to add batch scanId/source = {scanId}/{source} / {url}");
                throw;
            }
        }
    }
}
