using System.Collections.Generic;
using System.Threading.Tasks;
using Realt.Parser.Core.Model;

namespace Realt.Parser.Core
{
    public interface IParser
    {
        int Source { get; }
        IEnumerable<Search> GetSearchSequence();
        Task<Info> GetInfoAsync(Search search);
        Task<IEnumerable<Property>> ReadPageAsync(Search search, int index);
    }
}
