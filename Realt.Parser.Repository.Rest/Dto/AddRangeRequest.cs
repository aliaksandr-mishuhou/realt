using System;
using Realt.Parser.Core.Model;

namespace Realt.Parser.Repository.Rest.Dto
{
    public class AddRangeRequest
    {
        public string ScanId { get; set; }
        public DateTime Scanned { get; set; }
        public int Source { get; set; }
        public Property[] Items;
    }
}
