using System.Threading.Tasks;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public interface IRepository
    {
        Task<bool> AddAsync(Property item);
        Task<bool> AddRangeAsync(Property[] items);
    }
}