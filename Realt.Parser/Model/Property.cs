using System;

namespace Realt.Parser.Model
{
    public class Property
    {
        public int Rooms { get; set; }
        public int Year { get; set; }
        public double SquareTotal { get; set; }
        public double SquareLiving { get; set; }
        public int Floor { get; set; }
        public string Address { get; set; }
        public int PriceUsd { get; set; }
        public int PriceByn { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
