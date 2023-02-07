using AutoMapper;
using Microsoft.Extensions.Logging;
using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.EventModels;
using Server.Common.Interfaces.Models.IEventModel;
using Server.Common.Interfaces.Models.IUserModel;
using Server.DAL.Models;

namespace Server.BLL.Managers.EventManager
{
    public class EventManager: IEventManager
    {
        private readonly IEventRepository<Event> _eventRepository;
        private readonly ILogger<EventManager> _log;
        private readonly IMapper _mapper;
        private readonly IUserRepository<User> _userRepository;

        public EventManager(IEventRepository<Event> eventRepository, ILogger<EventManager> log, IMapper mapper,
            IUserRepository<User> userRepository)
        {
            _eventRepository = eventRepository;
            _log = log;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public bool AddNewEventById(NewEventWithUserId newEvent)
        {
            if (!_userRepository.UserIsExist(newEvent.UserId))
            {
                return false;
            }
            Event addNewEvent = _mapper.Map<Event>(newEvent);
            _log.LogInformation($"Admin with id {newEvent.UserId} is trying to add new event");
            _eventRepository.Create(addNewEvent);
            _eventRepository.Save();
            return true;
        }

        public bool AddNewEventByUsername(NewEventWithUsername newEvent)
        {
            if (!_userRepository.UserIsExist(newEvent.Login))
            {
                return false;
            }
            Event addNewEvent = _mapper.Map<Event>(newEvent);
            addNewEvent.UserId = _userRepository.GetUserIdByName(newEvent.Login);
            _log.LogInformation($"Admin '{newEvent.Login}' is trying to add new event");
            _eventRepository.Create(addNewEvent);
            _eventRepository.Save();
            return true;
        }

        public List<EventModel> GetEventsForRelevantUser(string username)
        {
            if (!_userRepository.UserIsExist(username))
            {
                return null;
            }
            _log.LogInformation($"Extracting all added events by admin '{username}'");
            List<Event> events = _eventRepository.GetEventsForRelevantUser(_userRepository.GetUserIdByName(username)).ToList();
            List<EventModel> result = new List<EventModel>(events.Count);
            foreach (Event e in events)
            {
                result.Add(_mapper.Map<EventModel>(e));
            }
            return result;
        }

        public List<EventModel> GetEventsForRelevantUser(long id)
        {
            if (!_userRepository.UserIsExist(id))
            {
                return null;
            }
            _log.LogInformation($"Extracting all added events by admin with id '{id}'");
            List<Event> events = _eventRepository.GetEventsForRelevantUser(id).ToList();
            List<EventModel> result = new List<EventModel>(events.Count);
            foreach (Event e in events)
            {
                result.Add(_mapper.Map<EventModel>(e));
            }
            return result;
        }
    }
}
