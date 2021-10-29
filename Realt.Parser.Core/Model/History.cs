using System;

namespace Realt.Parser.Core.Model
{
    public class History : Property
    {
        public DateTime Scanned { get; set; }
        public string ScanId { get; set; }
    }
}
