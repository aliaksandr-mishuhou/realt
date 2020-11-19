using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace Realt.Parser.Model
{
    [Table("history")]
    public class HistoryProperty : Property
    {
        public DateTime Scanned { get; set; }
        public string ScanId { get; set; }

        public static HistoryProperty Create(Property property, string scanId, DateTime scanned)
        {
            var propertyHistory = JsonConvert.DeserializeObject<HistoryProperty>(JsonConvert.SerializeObject(property));
            propertyHistory.ScanId = scanId;
            propertyHistory.Scanned = scanned;
            return propertyHistory;
        }
    }
}
