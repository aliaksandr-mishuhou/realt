using System.Threading.Tasks;

namespace Realt.Parser
{
    public interface IRunner
    {
        Task RunV1Async();
        Task RunV2Async();
    }
}