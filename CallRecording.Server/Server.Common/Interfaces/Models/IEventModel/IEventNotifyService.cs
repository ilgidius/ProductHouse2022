using Server.Common.Classes.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Interfaces.Models.IEventModel
{
    public interface IEventNotifyService
    {
        void AddSubscriber(string login, string eventType);
        void RemoveSubscriber(string login);
        void ChangeSubscription(string login, string eventType);
        void AddNotification(EventModel eventToPublish);
        Task<string> PublishAsync(string login);
    }
}
