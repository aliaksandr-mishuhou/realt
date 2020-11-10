using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public class JsonPageFileRepository : IRepository
    {
        private const string DirectoryTemplate = "./data/{0}/json";
        private const string FilenameTemplate = "{0}_{1}.json";

        public async Task<bool> AddRangeAsync(IEnumerable<Property> items, int operationId)
        {
            var dateStr = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var filename = string.Format(FilenameTemplate, dateStr, operationId);
            var directory = string.Format(DirectoryTemplate, dateStr);
            var path = $"{directory}/{filename}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var content = JsonConvert.SerializeObject(items);
            await File.WriteAllTextAsync(path, content);
            return true;
        }
    }
}
