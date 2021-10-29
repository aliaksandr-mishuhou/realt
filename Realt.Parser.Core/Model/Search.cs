namespace Realt.Parser.Core.Model
{
    public class Search
    {
        public string Token { get; set; }

        public int[] Rooms { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }

        public override string ToString()
        {
            var roomsStr = Rooms != null ? string.Join(",", Rooms) : null;
            return $"years:{YearFrom}-{YearTo}, rooms:[{roomsStr}]";
        }
    }
}
