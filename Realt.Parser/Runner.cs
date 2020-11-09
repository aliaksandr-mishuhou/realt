﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Realt.Parser
{
    public class Runner : IRunner
    {
        private readonly IParser _parser;
        private readonly IRepository _repository;
        private readonly ILogger<Runner> _logger;

        public Runner(IParser parser, IRepository repository, ILogger<Runner> logger)
        {
            _parser = parser;
            _repository = repository;
            _logger = logger;
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
                    await _repository.AddRangeAsync(items, i.ToString());
                    _logger.LogInformation($"Page {i}: {items.Count()}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Fatal error", ex);
            }
        }

        //public Task ResumeAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
