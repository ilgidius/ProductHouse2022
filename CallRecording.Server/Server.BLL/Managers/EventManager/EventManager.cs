using AutoMapper;
using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.EventModels;
using Server.Common.Interfaces.Models.IEventModel;
using Server.Common.Interfaces.Models.IUserModel;
using Server.DAL.Models;
using Server.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.BLL.Managers.EventManager
{
    public class EventManager: IEventManager
    {
        private readonly IEventRepository<Event, EventFilter> _eventRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository<User> _userRepository;

        public EventManager(IEventRepository<Event, EventFilter> eventRepository, IMapper mapper,
            IUserRepository<User> userRepository)
        {
            _eventRepository = eventRepository;
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
