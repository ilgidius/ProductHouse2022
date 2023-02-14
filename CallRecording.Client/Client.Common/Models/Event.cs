using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class Event
    {
        public string? AddedTime { get; private set; }
        public string? EventType { get; private set; }
        public string? Key { get; private set; }
        public string? Value { get; private set; }

        public Event(string eventType, string key, string value)
        {
            AddedTime = DateTime.Now.ToUniversalTime().ToString();
            EventType = eventType;
            Key = key;
            Value = value;
        }

        public Event()
        {
            AddedTime = DateTime.Now.ToUniversalTime().ToString();
            do
            {
                Console.Write("Enter event type: ");
                EventType = Console.ReadLine().ToUpper();
            } while (EventType != "INIT" && EventType != "RINGING" && EventType != "START" && EventType != "VOICE" && EventType != "STOP");
            Console.Write("Enter key: ");
            Key = Console.ReadLine();
            Console.Write("Enter value: ");
            Value = Console.ReadLine();
        }
    }
}
