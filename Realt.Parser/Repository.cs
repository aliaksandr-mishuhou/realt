using System;
using System.Threading.Tasks;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public class Repository : IRepository
    {
        public Task<bool> AddAsync(Property item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddRangeAsync(Property[] items)
        {
            throw new NotImplementedException();
        }
    }
}
