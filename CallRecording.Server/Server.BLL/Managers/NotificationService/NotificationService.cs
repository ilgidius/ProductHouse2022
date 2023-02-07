using Microsoft.Extensions.Logging;
using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.Notification;
using Server.Common.Interfaces.Models.INotification;
using System.Collections.Concurrent;

namespace Server.BLL.Managers.NotificationService
{
    public class NotificationService : INotificationService
    {
        private HashSet<SubscriptionContext> _subscribers = new HashSet<SubscriptionContext>();
        private ConcurrentQueue<EventModel> _events = new ConcurrentQueue<EventModel>();
        private readonly ILogger<NotificationService> _log;
        private readonly Thread _sendNotificationThread;

        public NotificationService(ILogger<NotificationService> log)
        {
            _log = log;
            _sendNotificationThread = new Thread(SentNotification);
            _sendNotificationThread.Start();
        }

        public void AddEventIntoQueue(EventModel newEvent)
        {
            _log.LogInformation($"Adding new event {newEvent.EventType}");
            _events.Enqueue(newEvent);
        }

        private async void SentNotification()
        {
            while(true)
            {
                if(await WaitToReadAsync())
                {
                    _events.TryDequeue(out EventModel? eventModel);
                    lock(_subscribers)
                    {
                        foreach(var subscriber in _subscribers)
                        {
                            if (subscriber.EventType == eventModel.EventType)
                            {
                                _log.LogInformation($"Event for '{subscriber.Login}' was sent");
                                subscriber.SentNotification(eventModel);
                            }
                        }
                    }
                    continue;
                }
            }
        }

        private async Task<bool> WaitToReadAsync()
        {
            while (true)
            {
                if (_events.Count > 0)
                {
                    return true;
                }
                await Task.Delay(1000);
            }
        }

        public void Subscribe(SubscriptionContext subscriptionContext)
        {
            if (subscriptionContext != null)
            {
                lock (_subscribers)
                {
                    _subscribers.Add(subscriptionContext);
                    _log.LogInformation($"New subscriber '{subscriptionContext.Login}' was subscribed");
                }
            }
        }

        public void Unsubscribe(SubscriptionContext subscriptionContext)
        {
            if (subscriptionContext != null)
            {
                lock (_subscribers)
                {
                    _subscribers.Remove(subscriptionContext);
                    _log.LogInformation($"Subscriber '{subscriptionContext.Login}' was unsubscribed");
                }
            }
        }

        public bool IsSubscribed(SubscriptionContext subscriptionContext)
        {
            lock (_subscribers)
            {
                foreach (var subcreber in _subscribers)
                {
                    if (subcreber.Login == subscriptionContext.Login)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
