namespace Realt.Parser.Model
{
    public class Info
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public string Token { get; set; }

        public override string ToString()
        {
            return $"total = {Total}, pages = {TotalPages}";
        }
    }
}
