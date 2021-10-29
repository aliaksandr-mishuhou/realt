using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Realt.Parser.Core
{
    public class DefaultWorker : BackgroundService
    {
        private readonly IRunner _runner;
        private readonly ILogger<DefaultWorker> _logger;

        private const int IntervalMs = 1000 * 60; // 1 min
        private const string DefaultExecutionTimeValue = "09:00:00;15:00:00;21:00:00";
        private const string ExecuteNowValue = "now";

        private const string ConfigExecutionTimeKey = "ExecutionTime";
        private IEnumerable<TimeSpan> _executionTimes;

        public DefaultWorker(IRunner runner, ILogger<DefaultWorker> logger, IConfiguration configuration)
        {
            _runner = runner;
            _logger = logger;
            
            var timesStr = configuration.GetValue<string>(ConfigExecutionTimeKey);
            if (string.IsNullOrEmpty(timesStr))
            {
                timesStr = DefaultExecutionTimeValue;
            }

            if (timesStr != ExecuteNowValue)
            { 
                _executionTimes = timesStr.Split(";")
                    .Select(s => TimeSpan.Parse(s)).ToList();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.UtcNow);

                if (_executionTimes == null)
                {
                    await _runner.RunAsync();
                    break;
                }

                var utc = DateTime.UtcNow;
                var currentTime = new TimeSpan(utc.Hour, utc.Minute, 0);
                if (_executionTimes.Contains(currentTime))
                {
                    await _runner.RunAsync();
                }

                await Task.Delay(IntervalMs, stoppingToken);
            }
        }
    }
}
