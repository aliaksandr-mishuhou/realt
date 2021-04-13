using System.Collections.Generic;
using System.Threading.Tasks;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public interface IParserV2
    {
        int Source { get; }
        Task<Info> GetInfoAsync(Search search);
        Task<IEnumerable<Property>> ReadPageAsync(Search search, int index);
    }
}
