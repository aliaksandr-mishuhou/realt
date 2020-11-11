using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public class CsvRepository : IRepository
    {
        private const string DirectoryTemplate = "./data/{0}";
        private const string FilenameTemplate = "{0}.csv";
        private const string CsvSeparator = ",";

        private readonly IEnumerable<string> _columns = TypeDescriptor
            .GetProperties(typeof(Property))
            .OfType<PropertyDescriptor>()
            .ToList()
            .Select(x => x.Name);

        public async Task<bool> AddRangeAsync(IEnumerable<Property> items, int operationId)
        {
            var dateStr = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var filename = string.Format(FilenameTemplate, dateStr);
            var directory = string.Format(DirectoryTemplate, dateStr);
            var path = $"{directory}/{filename}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (operationId == 0 && File.Exists(path))
            {
                File.Delete(path);
            }

            if (!File.Exists(path))
            {
                var header = string.Join(CsvSeparator, _columns);
                await File.AppendAllLinesAsync(path, new[] { header });
            }

            var lines = items.Select(row => BuildCsvRow(row));
            await File.AppendAllLinesAsync(path, lines);

            return true;
        }

        private string BuildCsvRow(Property property)
        {
            return string.Join(CsvSeparator,
                _columns.Select(a => EscapeCsvValue(typeof(Property)
                .GetProperty(a).GetValue(property, null))));
        }

        private static object EscapeCsvValue(object s)
        {
            if (s != null)
            {
                if (s is DateTime)
                {
                    return ((DateTime)s).ToString("yyyy-MM-dd");
                }

                if (s.ToString().Contains(CsvSeparator))
                {
                    return $"\"{s}\"";
                }
            }

            return s;
        }
    }

}
