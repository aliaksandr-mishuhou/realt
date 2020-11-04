using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public class Parser : IParser
    {
        private const string ParamsConfigPath = "./search.params";

        private const string FormUrl = "https://realt.by/sale/flats/search/";
        private const string CounterUrlTemplate = "https://realt.by/?eID=tx_uedbcore_mapApi&tid=1&R=0&type=count&hash={0}&{1}"; // hash, search params
        private const string PreSearchUrl = "https://realt.by/sale/flats/";
        private const string SearchUrlTemplate = "https://realt.by/sale/flats/?search={0}&view=0&page={1}"; // search token, page

        private readonly HttpClient _client = new HttpClient();

        public Task<Info> GetInfoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Property>> ReadPageAsync(string token, int index)
        {
            throw new NotImplementedException();
        }

    }
}
