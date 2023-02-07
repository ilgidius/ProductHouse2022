using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Server.BLL.Background
{
    public class PeriodicHostedService : BackgroundService
    {
        private TimeSpan _period;
        private readonly ILogger<PeriodicHostedService> _log;
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _factory;
        private int _executionCount = 0;
        public static bool IsEnabled { get; set; }

        public PeriodicHostedService(ILogger<PeriodicHostedService> log, IServiceScopeFactory factory,
            IConfiguration config)
        {
            _log = log;
            _factory = factory;
            _config = config;
            _period = TimeSpan.FromDays(Convert.ToDouble(_config["PeriodicService:Frequency:Days"])) +
                TimeSpan.FromHours(Convert.ToDouble(_config["PeriodicService:Frequency:Hours"])) +
                TimeSpan.FromMinutes(Convert.ToDouble(_config["PeriodicService:Frequency:Minutes"])) +
                TimeSpan.FromSeconds(Convert.ToDouble(_config["PeriodicService:Frequency:Seconds"]));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (!stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    if (IsEnabled)
                    {
                        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                        BackgroundPeriodicService sampleService = asyncScope.ServiceProvider.GetRequiredService<BackgroundPeriodicService>();
                        await sampleService.ClearOldEventsAsync(DateTime.Now - TimeSpan.FromDays(Convert.ToDouble(_config["PeriodicService:DeleteRowsOlderThan:Days"])) -
                            TimeSpan.FromHours(Convert.ToDouble(_config["PeriodicService:DeleteRowsOlderThan:Hours"])) -
                            TimeSpan.FromMinutes(Convert.ToDouble(_config["PeriodicService:DeleteRowsOlderThan:Minutes"])) -
                            TimeSpan.FromSeconds(Convert.ToDouble(_config["PeriodicService:DeleteRowsOlderThan:Seconds"])));
                        _executionCount++;
                        _log.LogInformation($"Executed PeriodicHostedService - Count: {_executionCount}");
                    }
                    else
                    {
                        _log.LogInformation("Skipped PeriodicHostedService");
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"Failed to execute PeriodicHostedService", ex);
                }
            }
        }
    }
}
