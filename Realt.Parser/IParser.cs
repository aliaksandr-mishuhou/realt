using System.Collections.Generic;
using System.Threading.Tasks;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public interface IParser
    {
        Task<Info> GetInfoAsync();
        Task<IEnumerable<Property>> ReadPageAsync(string token, int index);
    }
}