using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.EventModels;

namespace Server.Common.Interfaces.Models.IEventModel
{
    public interface IEventManager
    {
        bool AddNewEventById(NewEventWithUserId newEvent);
        bool AddNewEventByUsername(NewEventWithUsername newEvent);
        List<EventModel> GetEventsForRelevantUser(string username);
        List<EventModel> GetEventsForRelevantUser(long id);
    }
}
