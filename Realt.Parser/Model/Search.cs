namespace Realt.Parser.Model
{
    public class Search
    {
        public string Token { get; set; }

        public int[] Rooms { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }

        public override string ToString()
        {
            return $"years:{YearFrom}-{YearTo}, rooms:[{string.Join(",", Rooms)}], token={Token}";
        }
    }
}
