using Newtonsoft.Json;

namespace Realt.Parser.Onliner.Dto
{
    public class Page
    {
        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("items")]
        public int Items { get; set; }

        [JsonProperty("current")]
        public int Current { get; set; }

        [JsonProperty("last")]
        public int Last { get; set; }
    }
}
