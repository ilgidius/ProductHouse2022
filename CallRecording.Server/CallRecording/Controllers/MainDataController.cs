using CallRecording.Common.Repository;
using CallRecording.DAL.Models;
using CallRecording.DAL.Repository;
using CallRecording.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallRecording.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class MainDataController : ControllerBase
    {
        private readonly ILogger<MainDataController> _logger;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Event> _eventRepository;
        public MainDataController(ILogger<MainDataController> logger, IRepository<User> repository, IRepository<Event> eventRepository)
        {
            _logger = logger;
            _userRepository = repository;
            _eventRepository = eventRepository;
        }
        [HttpGet, Route("api/events")]
        public ActionResult<List<Event>> GetEvents()
        {
            List<Event> data = _eventRepository.GetList().ToList();
            _eventRepository.Dispose();
            if (data.Count == 0)
            {
                return NotFound();
            }
            return data;
        }

        [HttpGet, Route("api/events/{id}")]
        public ActionResult<Event> GetEventById(int id)
        {
            Event data = _eventRepository.GetById(id);
            _eventRepository.Dispose();
            if (data == null)
            {
                return NotFound();
            }
            return data;
        }

        [HttpGet, Route("api/users")]
        public ActionResult<List<User>> GetUsers()
        {
            var data = _userRepository.GetList().ToList();
            _userRepository.Dispose();
            if (data.Count == 0)
            {
                return NotFound();
            }
            return data;
        }

        [HttpGet, Route("api/users/{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            User data = _userRepository.GetById(id);
            _userRepository.Dispose();
            if (data == null)
            {
                return NotFound();
            }
            return data;
        }

        [HttpPost, Route("api/users/create")]
        public ActionResult<User> CreateUser([FromBody] CreateUserRequest userCreateData)
        {
            User newUser = new User()
            {
                Login = userCreateData.Username,
                Password = userCreateData.Password,
                Role = userCreateData.Role
            };
            _userRepository.Create(newUser);
            _userRepository.Dispose();
            return newUser;
        }
        [HttpPost, Route("api/users/update")]
        public ActionResult<User> UpdateUser([FromBody] CreateUserRequest userCreateData)
        {
            User updateUser = new User()
            {
                Login = userCreateData.Username,
                Password = userCreateData.Password,
                Role = userCreateData.Role
            };
            _userRepository.Update(updateUser);
            _userRepository.Dispose();
            return updateUser;
        }
        [HttpPost, Route("api/users/delete")]
        public ActionResult<User> DeleteUser([FromBody] CreateUserRequest userCreateData)
        {
            User deleteUser = new User()
            {
                Login = userCreateData.Username,
                Password = userCreateData.Password,
                Role = userCreateData.Role
            };
            _userRepository.Update(deleteUser);
            _userRepository.Dispose();
            return deleteUser;
        }
        [HttpPost, Route("api/events/create")]
        public ActionResult<Event> CreateEvent([FromBody] CreateEventRequest eventCreateData)
        {
            Event newEvent = new Event()
            {
                UserId = _userRepository.GetById(1).Id,
                AddedTime = eventCreateData.AddedTime.ToString(),
                SentTime = eventCreateData.SentTime.ToString(),
                EventType = eventCreateData.EventType,
                Key = eventCreateData.BussinesData.Key,
                Value = eventCreateData.BussinesData.Value
            };
            _eventRepository.Create(newEvent);
            _eventRepository.Dispose();
            return newEvent;
        }
        [HttpPost, Route("api/events/update")]
        public ActionResult<Event> UpdateEvent([FromBody] CreateEventRequest eventCreateData)
        {
            Event updateEvent = new Event()
            {
                UserId = _userRepository.GetById(1).Id,
                AddedTime = eventCreateData.AddedTime.ToString(),
                SentTime = eventCreateData.SentTime.ToString(),
                EventType = eventCreateData.EventType,
                Key = eventCreateData.BussinesData.Key,
                Value = eventCreateData.BussinesData.Value
            };
            _eventRepository.Update(updateEvent);
            _eventRepository.Dispose();
            return updateEvent;
        }
        [HttpPost, Route("api/events/delete")]
        public ActionResult<Event> DeleteEvent([FromBody] CreateEventRequest eventCreateData)
        {
            Event deleteEvent = new Event()
            {
                UserId = _userRepository.GetById(1).Id,
                AddedTime = eventCreateData.AddedTime.ToString(),
                SentTime = eventCreateData.SentTime.ToString(),
                EventType = eventCreateData.EventType,
                Key = eventCreateData.BussinesData.Key,
                Value = eventCreateData.BussinesData.Value
            };
            _eventRepository.Delete(deleteEvent);
            _eventRepository.Dispose();
            return deleteEvent;
        }
    }
}
