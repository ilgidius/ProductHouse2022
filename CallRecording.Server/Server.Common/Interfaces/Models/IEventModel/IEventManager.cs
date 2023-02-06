using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Interfaces.Models.IEventModel
{
    public interface IEventManager
    {
        bool AddNewEventById(NewEventWithUserId newEvent);
        bool AddNewEventByUsername(NewEventWithUsername newEvent);
        List<Classes.Models.Common.EventModel> GetEventsForRelevantUser(string username);
        List<Classes.Models.Common.EventModel> GetEventsForRelevantUser(long id);
    }
}
