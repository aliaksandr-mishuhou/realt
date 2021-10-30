using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Realt.Parser.Core;
using Realt.Parser.Core.Model;
using Realt.Parser.OnlinerBy.Dto;

namespace Realt.Parser.OnlinerBy
{
    public class OnlinerByParser : IParser
    {
        private const string UrlTemplate = "https://pk.api.onliner.by/search/apartments"
            + "?bounds[lb][lat]=53.76903249787691&bounds[lb][long]=27.343844184391624"
            + "&bounds[rt][lat]=54.026899653566154&bounds[rt][long]=27.780963372426374"
            + "&v={0}&page={1}";
        private const string YearFromParam = "building_year[min]";
        private const string YearToParam = "building_year[max]";
        private const string RoomParam = "number_of_rooms[]";

        private const int SourceId = 2;

        private const int MaxItemsPerSearch = 96 * 8;

        private readonly Random _random = new Random(DateTime.UtcNow.Millisecond);

        private readonly HttpClient _client = new HttpClient();

        private readonly ILogger<OnlinerByParser> _logger;

        public int Source => SourceId;

        public OnlinerByParser(ILogger<OnlinerByParser> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Search> GetSearchSequence()
        {
            return new OnlinerBySequence();
        }

        public async Task<Info> GetInfoAsync()
        {
            var dto = await ReadPageInternalAsync(0);
            return ToInfo(dto);
        }

        public async Task<Info> GetInfoAsync(Search search)
        {
            var dto = await ReadPageInternalAsync(search, 0);
            return ToInfo(dto);
        }

        public async Task<IEnumerable<Property>> ReadPageAsync(string token, int index)
        {
            var dto = await ReadPageInternalAsync(index + 1);
            return ToProperties(dto);
        }

        public async Task<IEnumerable<Property>> ReadPageAsync(Search search, int index)
        {
            var dto = await ReadPageInternalAsync(search, index + 1);
            return ToProperties(dto);
        }

        private Task<Response> ReadPageInternalAsync(int index)
        {
            return ReadPageInternalAsync(new Search(), index);
        }

        private async Task<Response> ReadPageInternalAsync(Search search, int index)
        {
            try
            {
                var url = string.Format(UrlTemplate, _random.NextDouble(), index);
                var parameters = ToUrlParameters(search);
                foreach (var kv in parameters)
                {
                    url += $"&{kv.Key}={kv.Value}";
                }

                _logger.LogDebug(url);

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get,
                };

                var acceptHeader = request.Headers.Accept;
                acceptHeader.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                acceptHeader.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                acceptHeader.Add(new MediaTypeWithQualityHeaderValue("*/*"));

                var response = await _client.SendAsync(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"Could not read page = {index}: response = {response.StatusCode}");
                    return new Response();
                }

                var json = await response.Content.ReadAsStringAsync();

                var dto = JsonConvert.DeserializeObject<Response>(json);
                _logger.LogInformation($"Loaded page: {dto}");
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could not read page = {index}");
                return new Response();
            }
        }

        private int? ToPrice(Currency price)
        {
            if (price == null || string.IsNullOrEmpty(price.Amount))
            {
                return null;
            }

            return Convert.ToInt32(Convert.ToDouble(price.Amount));
        }

        private IEnumerable<Property> ToProperties(Response dto)
        {
            var properties = dto.Apartments?.Select(a => new Property
            {
                Id = a.Id,
                Address = a.Location.Address,
                PriceUsd = ToPrice(a.Price.Converted.USD),
                PriceByn = ToPrice(a.Price.Converted.BYN),
                Floor = a.Floor,
                FloorTotal = a.NumberOfFloors,
                RoomTotal = a.NumberOfRooms,
                SquareTotal = a.Area.Total,
                SquareLiving = a.Area.Living,
                SquareKitchen = a.Area.Kitchen,
                Created = a.CreatedAt,
                Source = SourceId
            });

            return properties;
        }

        private Info ToInfo(Response dto)
        {
            var info = new Info
            {
                Total = dto.Total,
                TotalPages = Convert.ToInt32(Math.Floor(dto.Total * 1f / dto.Page.Limit))
            };

            _logger.LogInformation($"Summary: {info}");
            if (info.Total > MaxItemsPerSearch)
            {
                _logger.LogWarning($"Total {info.Total} per search > {MaxItemsPerSearch}");
            }

            return info;
        }

        private IEnumerable<KeyValuePair<string, object>> ToUrlParameters(Search search)
        {
            var parameters = new List<KeyValuePair<string, object>>();

            if (search.YearFrom != null)
            {
                parameters.Add(new KeyValuePair<string, object>(YearFromParam, search.YearFrom));
            }
            if (search.YearTo != null)
            {
                parameters.Add(new KeyValuePair<string, object>(YearToParam, search.YearTo));
            }
            if (search.Rooms != null)
            {
                foreach (var r in search.Rooms)
                {
                    parameters.Add(new KeyValuePair<string, object>(RoomParam, r));
                }
            }

            return parameters;
        }
    }
}
