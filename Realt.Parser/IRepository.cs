using System.Collections.Generic;
using System.Threading.Tasks;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public interface IRepository
    {
        Task<bool> AddRangeAsync(IEnumerable<Property> items, string operationId);
    }
}