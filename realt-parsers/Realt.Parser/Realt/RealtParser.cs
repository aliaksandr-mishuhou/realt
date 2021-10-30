using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Realt.Parser.Model;

namespace Realt.Parser.Realt
{
    public class RealtParser : IParserV1, IParserV2
    {
        private const string ParamsConfigPath = "./search.params";

        private const string FormUrl = "https://realt.by/sale/flats/search/";
        private const string TotalUrlTemplate = "https://realt.by/?eID=tx_uedbcore_mapApi&tid=1&R=0&type=count&hash={0}&{1}";
        private const string SearchParamPrefix = "tx_uedbflat_pi2[DATA]";
        private const string TotalParamPrefix = "s";
        private const string PreSearchUrl = "https://realt.by/sale/flats/";
        private const string SearchUrlTemplate = "https://realt.by/sale/flats/?search={0}&view=0&page={1}"; // search token, page
        private const string PageParam = "tx_uedbflat_pi2[rec_per_page]";

        private const string YearFromParam = "tx_uedbflat_pi2[DATA][building_year][ge]";
        private const string YearToParam = "tx_uedbflat_pi2[DATA][building_year][le]";

        private const int Page = 50;
        private const string HashParam = "hash";

        private const int SourceId = 1;

        private readonly HttpClient _client = new HttpClient(
            new HttpClientHandler()
            {
                AllowAutoRedirect = false
            });

        private readonly HtmlParser _parser = new HtmlParser();
        private readonly ILogger<RealtParser> _logger;

        public int Source => SourceId;

        public RealtParser(ILogger<RealtParser> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Search> GetSearchSequence()
        {
            return new RealtSequence();
        }

        public Task<Info> GetInfoAsync()
        {
            return GetInfoAsync(null);
        }

        public async Task<Info> GetInfoAsync(Search search)
        {
            // load search params from config
            var formParams = await LoadSearchParamsAsync(search);

            // open form and parse 'hash'
            var hash = await GetHashAsync();
            formParams[HashParam] = hash;

            // set page size
            formParams[PageParam] = Page.ToString();

            // check total
            var total = await GetTotalAsync(formParams);

            // get search token from redirect location
            var token = await GetSearchTokenAsync(formParams);

            // finalize result
            return new Info
            {
                Token = token,
                Total = total,
                TotalPages = Convert.ToInt32(Math.Floor(total * 1f / Page))
            };
        }

        public Task<IEnumerable<Property>> ReadPageAsync(string token, int page)
        {
            return ReadPageAsync(new Search { Token = token }, page);
        }

        public async Task<IEnumerable<Property>> ReadPageAsync(Search search, int page)
        {
            var result = new List<Property>();
            var url = string.Format(SearchUrlTemplate, HttpUtility.UrlEncode(search.Token), page);

            _logger.LogDebug($"Loading {url}");

            var resultHtml = await _client.GetStringAsync(url);
            var document = await _parser.ParseDocumentAsync(resultHtml);
            var elements = document.QuerySelectorAll(".listing-item[data-mode=3]");
            foreach (var element in elements)
            {
                var property = BuildProperty(element);
                result.Add(property);
            }

            return result;
        }

        #region parse Info

        private async Task<string> GetSearchTokenAsync(Dictionary<string, string> formParams)
        {
            // build form request
            var formContent = new MultipartFormDataContent();

            foreach (var param in formParams)
            {
                var paramContent = new StringContent(param.Value);
                formContent.Add(paramContent, param.Key);
            }

            // post form and get redirect location
            var searchResponse = await _client.PostAsync(PreSearchUrl, formContent);

            // get search token from Location
            var searchUri = searchResponse.Headers.Location;
            var query = HttpUtility.ParseQueryString(searchUri.Query);
            var token = query.Get("search");
            return token;
        }

        private async Task<int> GetTotalAsync(Dictionary<string, string> formParams)
        {
            var searchQuery = HttpUtility.ParseQueryString(string.Empty);
            foreach (var param in formParams)
            {
                if (param.Key.StartsWith(SearchParamPrefix))
                {
                    searchQuery[param.Key.Replace(SearchParamPrefix, TotalParamPrefix)] = param.Value;
                }
            }

            var totalUrl = string.Format(TotalUrlTemplate, formParams["hash"], searchQuery.ToString());
            var json = await _client.GetStringAsync(totalUrl);
            var jObject = JsonConvert.DeserializeObject<JObject>(json);
            var total = jObject.GetValue("count").ToObject<int>();
            return total;
        }

        private static async Task<Dictionary<string, string>> LoadSearchParamsAsync(Search search = null)
        {
            // collect params
            var formParams = new Dictionary<string, string>();

            var lines = await File.ReadAllLinesAsync(ParamsConfigPath);
            foreach (var line in lines)
            {
                var param = line.Split(":");
                if (param.Length == 2)
                {
                    formParams[param[0]] = param[1];
                }
            }

            if (search != null)
            {
                UpdateSearchParameters(formParams, search);
            }

            return formParams;
        }

        private static void UpdateSearchParameters(Dictionary<string, string> parameters, Search search)
        {
            if (search.YearFrom != null)
            {
                parameters[YearFromParam] = search.YearFrom.ToString();
            }
            if (search.YearTo != null)
            {
                parameters[YearToParam] = search.YearTo.ToString();
            }
        }

        private async Task<string> GetHashAsync()
        {
            // open form
            var formHtml = await _client.GetStringAsync(FormUrl);

            // get hash 
            var document = await _parser.ParseDocumentAsync(formHtml);
            var hash = document.QuerySelector("#secret-hash").GetAttribute("value");

            return hash;
        }

        #endregion

        #region parse HTML item

        private Property BuildProperty(AngleSharp.Dom.IElement element)
        {
            var property = new Property
            {
                Source = SourceId
            };
            try
            {
                ParseId(element, property);
                ParseAddress(element, property);
                ParsePrices(element, property);

                ParseRooms(element, property);
                ParseFloor(element, property);
                ParseDate(element, property);
                ParseYear(element, property);
                ParseSquare(element, property);

                _logger.LogDebug("Completed [{0}]", property);
            }
            catch (Exception ex)
            {
                property.Error = ex.ToString();
                _logger.LogError(ex, "Could not transform [{0}]", property);
            }

            return property;
        }

        private void ParseSquare(AngleSharp.Dom.IElement element, Property property)
        {
            var m = Regex.Match(element.OuterHtml, @"([\d\.]{2,})\s(?:м)");
            if (m.Success)
            {
                property.SquareTotal = Convert.ToDouble(m.Groups[1].Value);
            }
        }

        private void ParseYear(AngleSharp.Dom.IElement element, Property property)
        {
            var m = Regex.Match(element.OuterHtml, @"(\d{4})(?:гп| г\.п\.)");
            if (m.Success)
            {
                property.Year = Convert.ToInt32(m.Groups[1].Value);
            }
        }

        private static void ParseId(AngleSharp.Dom.IElement element, Property property)
        {
            var m = Regex.Match(element.OuterHtml, @"(?:ID)\s(\d+)");
            if (m.Success)
            {
                var id = Convert.ToInt64(m.Groups[1].Value);
                property.Id = id;
            }
        }

        private static void ParseRooms(AngleSharp.Dom.IElement element, Property property)
        {
            var m = Regex.Match(element.OuterHtml, @"(\d+)(?:-комн\.)");
            if (m.Success) 
            {
                property.RoomTotal = Convert.ToInt32(m.Groups[1].Value);
                property.RoomSeparate = property.RoomTotal;
            }
        }

        private static void ParseAddress(AngleSharp.Dom.IElement element, Property property)
        {
            var m = Regex.Match(element.OuterHtml, @"([^><]+)\(([^\s\(\)]+)\s(?:район\))");
            if (!m.Success)
            {
                return;
            }

            property.Address = m.Groups[1].Value.Trim();
            property.District = m.Groups[2].Value.ToLower().Substring(0, 3);
        }

        private static void ParseFloor(AngleSharp.Dom.IElement element, Property property)
        {
            var m = Regex.Match(element.OuterHtml, @"(\d+)\/(\d+) (?:этаж)");
            if (m.Success)
            {
                property.Floor = ConvertFloor(m.Groups[1].Value);
                if (m.Groups.Count > 2)
                {
                    property.FloorTotal = ConvertFloor(m.Groups[2].Value);
                }
            }
        }

        private static int? ConvertFloor(string s)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch
            {
                return null;
            }
        }

        private static void ParseDate(AngleSharp.Dom.IElement element, Property property)
        {
            var m = Regex.Match(element.OuterHtml, @"(\d+)\.(\d+)\.(\d+)");
            if (m.Success)
            {
                property.Created = new DateTime(
                    Convert.ToInt32(m.Groups[3].Value),
                    Convert.ToInt32(m.Groups[2].Value),
                    Convert.ToInt32(m.Groups[1].Value));
            }
        }

        private void ParsePrices(AngleSharp.Dom.IElement element, Property property)
        {
            property.PriceByn = GetPrice(element, "BYN");
            property.PriceUsd = GetPrice(element, "USD");
        }

        private int? GetPrice(AngleSharp.Dom.IElement element, string currency)
        {
            var m = Regex.Match(element.OuterHtml, @$"([\d\s]+)(?:{currency})");
            if (m.Success)
            {
                return Convert.ToInt32(Regex.Replace(m.Groups[1].Value, @"\D", string.Empty));
            }

            return null;
        }

        #endregion
    }
}
