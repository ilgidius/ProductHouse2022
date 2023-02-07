using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.Notification;

namespace Server.Common.Interfaces.Models.INotification
{
    public interface INotificationService
    {
        void Subscribe(SubscriptionContext subscriptionContext);
        void Unsubscribe(SubscriptionContext subscriptionContext);
        bool IsSubscribed(SubscriptionContext subscriptionContext);
        void AddEventIntoQueue(EventModel newEvent);
    }
}
