using Newtonsoft.Json;

namespace Realt.Parser.Onliner.Dto
{
    public class Response
    {
        [JsonProperty("apartments")]
        public Apartment[] Apartments { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("page")]
        public Page Page { get; set; }

        public override string ToString() => $"total = {Total}, page = {Page.Current}, last = {Page.Last}, size = {Page.Limit}";
    }
}
