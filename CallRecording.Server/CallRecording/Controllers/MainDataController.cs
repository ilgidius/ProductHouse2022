using CallRecording.Common.IRepository;
using CallRecording.Common.IUser;
using CallRecording.DAL.Models;
using CallRecording.Mapping;
using CallRecording.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.CodeDom.Compiler;

namespace CallRecording.Controllers
{
    [ApiController]
    public class MainDataController : ControllerBase
    {
        private readonly ILogger<MainDataController> _logger;
        private readonly IConfiguration _configuration;

        private readonly IRepository<User> _userRepository;
        private readonly IUserRepository<User> _userRepositoryExtention;
        private readonly IUserValidation<User, UserLogin> _userValidation;

        private readonly IRepository<Event> _eventRepository;
        
        public MainDataController(ILogger<MainDataController> logger, IConfiguration config,
            IRepository<User> repository, IUserRepository<User> userRepository, IUserValidation<User, UserLogin> userValidation,
            IRepository<Event> eventRepository)
        {
            _logger = logger;
            _configuration = config;

            _userRepository = repository;
            _userRepositoryExtention = userRepository;
            _userValidation= userValidation;

            _eventRepository = eventRepository;
        }

        /*
         * User HTTP Get Requests
         */
        //[Authorize (Role = "admin")]
        [HttpGet, Route("api/users")]
        public ActionResult<List<User>> GetAllUsers()
        {
            _logger.LogInformation("Extracting all users...");
            var data = _userRepository.GetAll().ToList();
            if (data.Count == 0)
            {
                _logger.LogInformation("Users wasn't found");
                return NotFound();
            }
            _logger.LogInformation($"{data.Count} users was found");
            return data;
        }

        [HttpGet, Route("api/users/{id}")]
        public ActionResult<User> GetUserById(long id)
        {
            _logger.LogInformation($"Searching for {id} user...");
            User data = _userRepository.GetById(id);
            if (data == null)
            {
                _logger.LogInformation($"User {id} wasn't found");
                return NotFound();
            }
            _logger.LogInformation($"User {id} was found");
            return data;
        }

        /*
         * User HTTP Post Requests
         */
        [AllowAnonymous]
        [HttpPost, Route("api/users/login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = _userValidation.Authentificate(userLogin);

            if(user == null)
            {
                return NotFound("User not found.");
            }
            var tocken = _userValidation.Generate(user);
            return Ok(tocken);
        }



        [HttpGet, Route("api/events")]
        public ActionResult<List<Event>> GetAllEvents()
        {
            _logger.LogInformation("Extracting all events...");
            var data = _eventRepository.GetAll().ToList();
            if (data.Count == 0)
            {
                _logger.LogInformation("Events wasn't found");
                return NotFound();
            }
            _logger.LogInformation($"{data.Count} events was found");
            return data;
        }

        [HttpGet, Route("api/events/{id}")]
        public ActionResult<Event> GetEventById(long id)
        {
            _logger.LogInformation($"Searching for {id} event...");
            Event data = _eventRepository.GetById(id);
            if (data == null)
            {
                _logger.LogInformation($"Event {id} wasn't found");
                return NotFound();
            }
            _logger.LogInformation($"Event {id} was found");
            return data;
        }

        

        [HttpPost, Route("api/users/create")]
        public ActionResult<User> CreateUser([FromBody] CreateUserRequest userCreateData)
        {
            //User newUser = new User()
            //{
            //    Login = userCreateData.Username,
            //    Password = userCreateData.Password,
            //    Role = userCreateData.Role
            //};
            User newUser = ToUser.CreateUserRequestToUser(userCreateData);
            _userRepository.Create(newUser);
            _userRepository.Save();
            _logger.LogInformation($"User was added");
            return newUser;
        }
        [HttpPost, Route("api/users/update")]
        public ActionResult<User> UpdateUser([FromBody] CreateUserRequest userUpdateData)
        {
            //User updateUser = new User()
            //{
            //    Login = userUpdateData.Username,
            //    Password = userUpdateData.Password,
            //    Role = userUpdateData.Role
            //};
            User updateUser = ToUser.CreateUserRequestToUser(userUpdateData);
            _userRepository.Update(updateUser);
            _userRepository.Save();
            _logger.LogInformation($"User was updated");
            return updateUser;
        }
        [HttpDelete, Route("api/users")]
        public ActionResult<User> DeleteUser([FromBody] CreateUserRequest userDeleteData)
        {
            //User deleteUser = new User()
            //{
            //    Login = userDeleteData.Username,
            //    Password = userDeleteData.Password,
            //    Role = userDeleteData.Role
            //};
            User deleteUser = ToUser.CreateUserRequestToUser(userDeleteData);
            _userRepository.Delete(2);
            _userRepository.Save();
            _logger.LogInformation($"User was deleted");
            return deleteUser;
        }
        [HttpPost, Route("api/events/create")]
        public ActionResult<Event> CreateEvent([FromBody] CreateEventRequest eventCreateData)
        {
            //Event newEvent = new Event()
            //{
            //    UserId = _userRepository.GetById(1).Id,
            //    AddedTime = eventCreateData.AddedTime.ToString(),
            //    SentTime = eventCreateData.SentTime.ToString(),
            //    EventType = eventCreateData.EventType,
            //    Key = eventCreateData.BussinesData.Key,
            //    Value = eventCreateData.BussinesData.Value
            //};

            Event newEvent = ToEvent.CreateEventRequestToUser(eventCreateData, _userRepository.GetById(1).Id);
            _eventRepository.Create(newEvent);
            _userRepository.Save();
            _logger.LogInformation($"Event was created");
            return newEvent;
        }
        [HttpPost, Route("api/events/update")]
        public ActionResult<Event> UpdateEvent([FromBody] CreateEventRequest eventUpdateData)
        {
            //Event updateEvent = new Event()
            //{
            //    UserId = _userRepository.GetById(1).Id,
            //    AddedTime = eventUpdateData.AddedTime.ToString(),
            //    SentTime = eventUpdateData.SentTime.ToString(),
            //    EventType = eventUpdateData.EventType,
            //    Key = eventUpdateData.BussinesData.Key,
            //    Value = eventUpdateData.BussinesData.Value
            //};
            Event updateEvent = ToEvent.CreateEventRequestToUser(eventUpdateData, _userRepository.GetById(1).Id);
            _eventRepository.Update(updateEvent);
            _userRepository.Save();
            _logger.LogInformation($"Event was updated");
            return updateEvent;
        }
        [HttpPost, Route("api/events/delete")]
        public ActionResult<Event> DeleteEvent([FromBody] CreateEventRequest eventDeleteData)
        {
            //Event deleteEvent = new Event()
            //{
            //    UserId = _userRepository.GetById(1).Id,
            //    AddedTime = eventDeleteData.AddedTime.ToString(),
            //    SentTime = eventDeleteData.SentTime.ToString(),
            //    EventType = eventDeleteData.EventType,
            //    Key = eventDeleteData.BussinesData.Key,
            //    Value = eventDeleteData.BussinesData.Value
            //};
            Event deleteEvent = ToEvent.CreateEventRequestToUser(eventDeleteData, _userRepository.GetById(1).Id);
            _eventRepository.Delete(1);
            _userRepository.Save();
            _logger.LogInformation($"Event was deleted");
            return deleteEvent;
        }
    }
}
