using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Realt.Parser.Core.Model;

namespace Realt.Parser.Core
{
    public interface IRepository
    {
        Task ClearAsync(string scanId, int source);
        Task<int> AddRangeAsync(IEnumerable<Property> items, string scanId, DateTime scanned, int source);
    }
}