using Microsoft.Extensions.Logging;
using Server.Common.Interfaces.Models.IEventModel;
using Server.DAL.Models;

namespace Server.BLL.Background
{
    public class BackgroundPeriodicService
    {
        private readonly ILogger<BackgroundPeriodicService> _log;
        private readonly IEventRepository<Event> _eventRepository;

        public BackgroundPeriodicService(ILogger<BackgroundPeriodicService> log, IEventRepository<Event> eventRepository)
        {
            _log = log;
            _eventRepository = eventRepository;
        }

        public async Task ClearOldEventsAsync(DateTime date)
        {
            await Task.Run(() =>
            {
                _eventRepository.DeleteEventsOlderThan(date);
                _eventRepository.Save();
                _log.LogInformation($"Events older than {date} were deleted");
            });
        }
    }
}
