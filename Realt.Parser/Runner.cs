using System;
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

        public Task RunAsync()
        {
            throw new NotImplementedException();
        }

        //public Task ResumeAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
