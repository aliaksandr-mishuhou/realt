using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using AngleSharp.Html.Parser;
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

        private readonly HttpClient _client = new HttpClient(
            new HttpClientHandler()
            {
                AllowAutoRedirect = false
            });

        private readonly HtmlParser _parser = new HtmlParser();


        public async Task<Info> GetInfoAsync()
        {
            // load search params from config
            var formParams = await LoadSearchParamsAsync();

            // open form and parse 'hash'
            var hash = await GetHashAsync();
            formParams["hash"] = hash;

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

            Console.WriteLine(url);

            var resultHtml = await _client.GetStringAsync(url); // .bd-table-item-header
            var document = await _parser.ParseDocumentAsync(resultHtml);
            var elements = document.QuerySelectorAll(".bd-table-item-header");
            foreach (var element in elements)
            {
                var property = new Property();
                // const rooms = item.find('.kv').text();
                // const district = item.find('.ra').text();
                // const address = item.find('.ad').text();
                result.Add(property);
            }

            return result;
        }

    }
}
