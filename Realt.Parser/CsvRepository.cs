using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public class CsvRepository : IRepository
    {
        private const string DirectoryTemplate = "./data/{0}";
        private const string FilenameTemplate = "{0}.csv";
        private const string CsvSeparator = ",";

        public async Task<bool> AddRangeAsync(IEnumerable<Property> items, string operationId)
        {
            var dateStr = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var filename = string.Format(FilenameTemplate, dateStr);
            var directory = string.Format(DirectoryTemplate, dateStr);
            var path = $"{directory}/{filename}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var lines = items.Select(i => ToCsvLine(i));
            await File.AppendAllLinesAsync(path, lines);

            return true;
        }

        private static string ToCsvLine(Property property)
        {
            var properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(property));
            return string.Join(CsvSeparator, properties.Values);
        }

    }

}
