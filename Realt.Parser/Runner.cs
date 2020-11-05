using System;
using System.Linq;
using System.Threading.Tasks;

namespace Realt.Parser
{
    public class Runner : IRunner
    {
        private readonly IParser _parser;
        private readonly IRepository _repository;

        public Runner(IParser parser, IRepository repository)
        {
            _parser = parser;
            _repository = repository;
        }

        public async Task RunAsync()
        {
            try
            {
                // load general info (total & token)
                var info = await _parser.GetInfoAsync();
                Console.WriteLine($"Total: {info.Total}, Pages: {info.TotalPages}, Token: {info.Token}");

                for (var i = 0; i < info.TotalPages; i++)
                {
                    var items = await _parser.ReadPageAsync(info.Token, i);
                    Console.WriteLine($"Page {i}: {items.Count()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //public Task ResumeAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
