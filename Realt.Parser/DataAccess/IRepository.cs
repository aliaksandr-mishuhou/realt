using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Realt.Parser.Model;

namespace Realt.Parser.DataAccess
{
    public interface IRepository
    {
        Task ClearAsync(string scanId, int source);
        Task<bool> AddRangeAsync(IEnumerable<Property> items, string scanId, DateTime scanned);
    }
}