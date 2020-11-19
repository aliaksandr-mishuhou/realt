using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace Realt.Parser.Model
{
    public class Property
    {
        public long Id { get; set; }
        [Computed]
        public string Url { get; set; }

        public int? RoomTotal { get; set; }
        public int? RoomSeparate { get; set; }
        [Computed]
        public bool Shared { get; set; }
        public int? Year { get; set; }
        public double SquareTotal { get; set; }
        public double? SquareLiving { get; set; }
        public double? SquareKitchen { get; set; }
        public int? Floor { get; set; }
        public int? FloorTotal { get; set; }
        public string Type { get; set; }
        public string Balcony { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public int? PriceUsd { get; set; }
        public int? PriceByn { get; set; }
        public DateTime Created { get; set; }

#if DEBUG
        public string Error { get; set; }
#endif

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
