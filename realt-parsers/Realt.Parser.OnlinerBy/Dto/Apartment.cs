using System;
using Newtonsoft.Json;

namespace Realt.Parser.OnlinerBy.Dto
{
    public class Apartment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("author_id")]
        public int AuthorId { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("resale")]
        public bool Resale { get; set; }

        [JsonProperty("number_of_rooms")]
        public int NumberOfRooms { get; set; }

        [JsonProperty("floor")]
        public int Floor { get; set; }

        [JsonProperty("number_of_floors")]
        public int NumberOfFloors { get; set; }

        [JsonProperty("area")]
        public Area Area { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("seller")]
        public Seller Seller { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("last_time_up")]
        public DateTime LastTimeUp { get; set; }

        [JsonProperty("up_available_in")]
        public int UpAvailableIn { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("auction_bid")]
        public object AuctionBid { get; set; }
    }

    public class Location
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("user_address")]
        public string UserAddress { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }

    public class Currency
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Name { get; set; }
    }

    public class Converted
    {
        [JsonProperty("BYN")]
        public Currency BYN { get; set; }

        [JsonProperty("USD")]
        public Currency USD { get; set; }
    }

    public class Price
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("converted")]
        public Converted Converted { get; set; }
    }

    public class Area
    {
        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("living")]
        public double? Living { get; set; }

        [JsonProperty("kitchen")]
        public double? Kitchen { get; set; }
    }

    public class Seller
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
