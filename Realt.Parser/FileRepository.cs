using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public class FileRepository : IRepository
    {
        private const string FilenameTemplate = "./data/db-{0}-{1}.json";

        public async Task<bool> AddRangeAsync(IEnumerable<Property> items, string operationId)
        {
            var filename = string.Format(FilenameTemplate, DateTime.UtcNow.ToString("yyyy-MM-dd"), operationId);
            var content = JsonConvert.SerializeObject(items);
            await File.WriteAllTextAsync(filename, content);
            return true;
        }
    }
}
