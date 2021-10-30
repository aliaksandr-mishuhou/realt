using System;
using Newtonsoft.Json;

namespace Realt.Parser.Model
{
    public class History : Property
    {
        public DateTime Scanned { get; set; }
        public string ScanId { get; set; }

        public static History Create(Property property, string scanId, DateTime scanned)
        {
            var propertyHistory = JsonConvert.DeserializeObject<History>(JsonConvert.SerializeObject(property));
            propertyHistory.ScanId = scanId;
            propertyHistory.Scanned = scanned;
            return propertyHistory;
        }
    }
}
