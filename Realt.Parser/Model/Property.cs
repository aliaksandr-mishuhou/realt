using System;
using Newtonsoft.Json;

namespace Realt.Parser.Model
{
    public class Property
    {
        public long Id { get; set; }
        public int RoomTotal { get; set; }
        public int RoomLiving { get; set; }
        public int Year { get; set; }
        public double SquareTotal { get; set; }
        public double? SquareLiving { get; set; }
        public double? SquareKitchen { get; set; }
        public int Floor { get; set; }
        public int FloorTotal { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public int PriceUsd { get; set; }
        public int PriceByn { get; set; }
        public DateTimeOffset Created { get; set; }

#if DEBUG
        public string Error { get; set; }
#endif

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
