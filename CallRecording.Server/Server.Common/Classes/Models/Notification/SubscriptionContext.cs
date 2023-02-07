using Server.Common.Classes.Models.Common;

namespace Server.Common.Classes.Models.Notification
{
    public class SubscriptionContext
    {
        public string? Login { get; set; }
        public string? EventType { get; set; }
        public event Func<EventModel, Task>? RiseNotificationEventAsync;
        public void SentNotification(EventModel sentEvent)
        {
            if (RiseNotificationEventAsync != null)
            {
                RiseNotificationEventAsync(sentEvent);
            }
        }
    }
}
