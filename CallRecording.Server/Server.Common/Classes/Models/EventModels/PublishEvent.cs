using Server.Common.Classes.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Classes.Models.EventModels
{
    public class PublishEvent : EventModel
    {
        public bool IsPublished { get; set; }

        public bool Equals(EventModel model)
        {
            return (EventType == model.EventType && AddedTime == model.AddedTime && SentTime == model.SentTime &&
            Key == model.Key && Value == model.Value);
        }
    }
}
