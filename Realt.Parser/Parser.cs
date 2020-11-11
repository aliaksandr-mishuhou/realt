using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using AngleSharp.Html.Parser;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public class Parser : IParser
    {
        private const string ParamsConfigPath = "./search.params";

        private const string FormUrl = "https://realt.by/sale/flats/search/";
        private const string TotalUrlTemplate = "https://realt.by/?eID=tx_uedbcore_mapApi&tid=1&R=0&type=count&hash={0}&{1}";
        private const string SearchParamPrefix = "tx_uedbflat_pi2[DATA]";
        private const string TotalParamPrefix = "s";
        private const string PreSearchUrl = "https://realt.by/sale/flats/";
        private const string SearchUrlTemplate = "https://realt.by/sale/flats/?search={0}&view=0&page={1}"; // search token, page
        private const string PageParam = "tx_uedbflat_pi2[rec_per_page]";
        private const int Page = 50;
        private const string HashParam = "hash";
        private const int CurrencyByn = 933;
        private const int CurrencyUsd = 840;
        private const string RoomShared = "к";

        private readonly HttpClient _client = new HttpClient(
            new HttpClientHandler()
            {
                AllowAutoRedirect = false
            });

        private readonly HtmlParser _parser = new HtmlParser();
        private readonly ILogger<Parser> _logger;

        public Parser(ILogger<Parser> logger)
        {
            _logger = logger;
        }


        public async Task<Info> GetInfoAsync()
        {
            // load search params from config
            var formParams = await LoadSearchParamsAsync();

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

        private static async Task<Dictionary<string, string>> LoadSearchParamsAsync()
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

            return formParams;
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

        public async Task<IEnumerable<Property>> ReadPageAsync(string token, int page)
        {
            var result = new List<Property>();
            var url = string.Format(SearchUrlTemplate, HttpUtility.UrlEncode(token), page);

            _logger.LogInformation($"Loading {url}");

            var resultHtml = await _client.GetStringAsync(url);
            var document = await _parser.ParseDocumentAsync(resultHtml);
            var elements = document.QuerySelectorAll(".bd-table-item");
            foreach (var element in elements)
            {
                Property property = BuildProperty(element);
                result.Add(property);
            }

            return result;
        }

        private Property BuildProperty(AngleSharp.Dom.IElement element)
        {
            var property = new Property();
            try
            {
                // .bd-table-item-header
                ParseId(element, property);

                ParseRooms(element, property);

                ParseAddress(element, property);

                ParseFloor(element, property);

                ParseYearSquareBalcony(element, property);

                ParsePrices(element, property);

                // .bd-table-item-wrapper
                ParseDate(element, property);

                _logger.LogInformation("Completed [{0}]", property);
            }
            catch (Exception ex)
            {
                property.Error = ex.ToString();
                _logger.LogError("Could not transform [{0}]", property, ex);
            }

            return property;
        }

        private static void ParseId(AngleSharp.Dom.IElement element, Property property)
        {
            var url = element.QuerySelector(".bd-table-item-header div.ad a").GetAttribute("href");
            property.Url = url;

            var m = Regex.Match(url, @"\/(\d+)\/");
            if (m.Success)
            {
                var id = Convert.ToInt64(m.Groups[1].Value);
                property.Id = id;
            }
        }

        private static void ParseRooms(AngleSharp.Dom.IElement element, Property property)
        {
            var rooms = element.QuerySelector(".bd-table-item-header div.kv span").InnerHtml.Trim().Split("/");
            if (rooms.Length == 2)
            {
                if (rooms[0] == RoomShared)
                {
                    property.Shared = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(rooms[0]))
                    {
                        property.RoomTotal = Convert.ToInt32(rooms[0]);
                    }

                    if (!string.IsNullOrEmpty(rooms[1]))
                    {
                        property.RoomSeparate = Convert.ToInt32(rooms[1]);
                    }
                }
            }
        }

        private static void ParseAddress(AngleSharp.Dom.IElement element, Property property)
        {
            var district = element.QuerySelector(".bd-table-item-header div.ra span").InnerHtml.Trim();
            property.District = district;

            var address = element.QuerySelector(".bd-table-item-header div.ad a").InnerHtml.Trim();
            property.Address = address;
        }

        private static void ParseYearSquareBalcony(AngleSharp.Dom.IElement element, Property property)
        {
            var pls = element.QuerySelectorAll(".bd-table-item-header div.pl span");
            if (pls.Length >= 3)
            {
                // square
                var square = pls[0].InnerHtml.Trim().Split("/");
                if (square.Length == 3)
                {
                    property.SquareTotal = ConvertSquare(square[0]).GetValueOrDefault();
                    property.SquareLiving = ConvertSquare(square[1]);
                    property.SquareKitchen = ConvertSquare(square[2]);
                }

                // year
                var year = pls[1].InnerHtml.Trim();
                var m = Regex.Match(year, @"\d{4}");
                if (m.Success)
                {
                    property.Year = Convert.ToInt32(m.Groups[0].Value);
                }

                // balcony
                property.Balcony = pls[2].InnerHtml.Trim();
            }
        }

        private static void ParseFloor(AngleSharp.Dom.IElement element, Property property)
        {
            var floor = element.QuerySelector(".bd-table-item-header div.ee span").InnerHtml.Trim();
            var m = Regex.Match(floor, @"(\d+)\w*\/(\d*) (\w*)");
            if (m.Success)
            {
                property.Floor = ConvertFloor(m.Groups[1].Value);
                if (m.Groups.Count > 2)
                {
                    property.FloorTotal = ConvertFloor(m.Groups[2].Value);
                }

                if (m.Groups.Count > 3)
                {
                    property.Type = m.Groups[3].Value;
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
            var date = element.QuerySelector(".bd-table-item-wrapper .date span").InnerHtml.Trim().Split(".");
            property.Created = new DateTime(
                Convert.ToInt32(date[2]),
                Convert.ToInt32(date[1]),
                Convert.ToInt32(date[0]));
        }

        private void ParsePrices(AngleSharp.Dom.IElement element, Property property)
        {
            property.PriceByn = GetPrice(element, CurrencyByn);
            property.PriceUsd = GetPrice(element, CurrencyUsd);
        }

        private int? GetPrice(AngleSharp.Dom.IElement element, int currency)
        {
            var priceElement = element.QuerySelector("span.price-switchable");
            if (priceElement == null)
            {
                return null;
            }

            var valueRaw = priceElement.GetAttribute($"data-{currency}");
            if (string.IsNullOrEmpty(valueRaw))
            {
                return null;
            }

            valueRaw = HttpUtility.HtmlDecode(valueRaw).Trim().Replace("\t", string.Empty);
            valueRaw = Regex.Replace(valueRaw, @"[\D]", string.Empty);

            return Convert.ToInt32(valueRaw);
        }

        private static double? ConvertSquare(string square)
        {
            try
            {
                return Convert.ToDouble(square.Replace(",", "."));
            }
            catch
            {
                return null;
            }
        }

        private static int? ConvertRoom(string price)
        {
            try
            {
                return Convert.ToInt32(price);
            }
            catch
            {
                return null;
            }
        }

        /*
          
        <div class="bd-table-item-header">
            <div class="kv" style="width:5%;"> <span> 1/1 </span> </div>
            <!-- kv -->
            <div class="ra" style="width:5%;"> <span>Мос</span> </div>
            <!-- ra -->
            <div class="ad" style="width:30%;"> <a href="https://realt.by/sale/flats/object/1974774/" title="Минск, Карпова ул., 16" target="_blank">Минск, Карпова ул., 16</a>
            </div>
            <!-- ad -->
            <div class="ee" style="width:10%;"> <span> 5/5 б </span> </div>
            <!-- ee -->
            <div class="pl" style="width:16%;"> <span>30/17,6/6</span> </div>
            <!-- pl -->
            <div class="pl" style="width:7%;"> <span> 1965 <br>2020 </span> </div>
            <!-- pl -->
            <div class="pl" style="width:7%;"> <span> бз </span> </div>
            <!-- pl --><div class="cena" style="width:20%;">
            <span class="price-switchable" data-0="42&amp;nbsp;000&amp;nbsp;$" data-840="42&amp;nbsp;000&amp;nbsp;$" data-978="35&amp;nbsp;457&amp;nbsp;€" data-112="1&amp;nbsp;083&amp;nbsp;млн&amp;nbsp;руб" data-933="108&amp;nbsp;276&amp;nbsp;руб"> ="1&amp;nbsp;182&amp;nbsp;€/кв.м." data-112="36,1&amp;nbsp;млн&amp;nbsp;руб/кв.м." data-933="3&amp;nbsp;609&amp;nbsp;руб/кв.м." data-643="108&amp;nbsp;173&amp;nbsp;<i class=&quot;fa fa-rub&quot;></i>/кв.м." data-nohide="1"> 1&nbsp;400&nbsp;$/кв.м. </span> </div>
            <!-- cena -->
        </div>
         */

    }
}
