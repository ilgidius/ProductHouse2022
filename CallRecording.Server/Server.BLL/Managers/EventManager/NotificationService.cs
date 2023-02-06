using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;
using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.UserModels;
using Server.Common.Interfaces.Models.IEventModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.BLL.Managers.EventManager
{
    public class NotificationService : IEventNotifyService
    {
        private HashSet<Subscriber> _subscribers = new HashSet<Subscriber>();
        private readonly ILogger _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public void AddSubscriber(string login, string eventType)
        {
            _subscribers.Add(new Subscriber(login, eventType));
            _logger.LogInformation($"User '{login}' was subscribed to '{eventType}' events");
        }

        public void ChangeSubscription(string login, string eventType)
        {
            Subscriber? sub = _subscribers.FirstOrDefault(s => s.Equals(login));
            _logger.LogInformation($"User '{login}' changed subscription from '{sub.EventType}' to '{eventType}'");
            sub?.ChangeEventType(eventType);
        }

        public void AddNotification(EventModel eventToPublish)
        {
            foreach (var subscriber in _subscribers)
            {
                if (subscriber.EventType == eventToPublish.EventType)
                {
                    subscriber.AddEvent(eventToPublish);
                    _logger.LogInformation($"Notification for '{subscriber.Login}' was added");
                }
            }
        }

        public async Task<string> PublishAsync(string login)
        {
            Subscriber? sub = _subscribers.FirstOrDefault(s => s.Equals(login));
            if (sub == null)
            {
                throw new NullReferenceException();
            }
            await Task.Run(() =>
            {
                while (sub.EventsCount == 0)
                {
                    Task.Delay(3000);
                }
                _logger.LogInformation($"Notification for '{sub.Login}' was detected");
            });
            EventModel sentEvent = sub.GetEventToPublish();
            sub.ClearPublishedEvents();
            string result = $"New event was added:\nEvent type: {sentEvent.EventType};\nAdded time: {sentEvent.AddedTime}\n" +
                $"Sent time: {sentEvent.SentTime};\nBussiness logic: \n\tKey: {sentEvent.Key};\n\tValue: {sentEvent.Value};\nEnd of message.";
            _logger.LogInformation($"Notification for '{sub.Login}' is sending");
            return result;
        }

        public void RemoveSubscriber(string login)
        {
            Subscriber? sub = _subscribers.FirstOrDefault(s => s.Equals(login));
            _logger.LogInformation($"Removing '{sub.Login}' from subscribers list");
            if (sub != null) _subscribers.Remove(sub);
        }
    }
}
