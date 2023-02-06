using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Classes.Models.UserModels
{
    public class Subscriber
    {
        public readonly string Login;
        private List<PublishEvent> _events;
        public string EventType { get; private set; }
        public int EventsCount { get; private set; }

        public Subscriber(string login, string eventType)
        {
            Login = login;
            EventType = eventType;
            _events = new List<PublishEvent>();
            EventsCount = 0;
        }

        public void AddEvent(EventModel newEvent)
        {
            if (newEvent.EventType == EventType)
            {
                _events.Add(new PublishEvent
                {
                    IsPublished = false,
                    AddedTime = newEvent.AddedTime,
                    EventType = newEvent.SentTime,
                    SentTime = newEvent.SentTime,
                    Key = newEvent.Key,
                    Value = newEvent.Value
                });
                EventsCount++;
            }
        }

        public void ClearPublishedEvents()
        {
            for (int i = 0; i < EventsCount; i++)
            {
                if (_events[i].IsPublished)
                {
                    _events.Remove(_events[i]);
                    EventsCount--;
                    i--;
                }
            }
        }

        private void EventIsPublished(EventModel publishedEvent)
        {
            PublishEvent? ev = _events.FirstOrDefault(u => u.Equals(publishedEvent));
            if (ev != null) { ev.IsPublished = true; }
        }

        public void ChangeEventType(string newEventType)
        {
            EventType = newEventType;
        }

        public bool Equals(string login)
        {
            return login == Login;
        }

        public EventModel GetEventToPublish()
        {
            EventModel sentEvent = new EventModel()
            {
                EventType = _events[0].EventType,
                AddedTime = _events[0].AddedTime,
                SentTime = _events[0].SentTime,
                Key = _events[0].Key,
                Value = _events[0].Value
            };
            _events[0].IsPublished = true;
            return sentEvent;
        }
    }
}
