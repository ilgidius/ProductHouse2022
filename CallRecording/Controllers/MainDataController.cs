using CallRecording.DAL.Models;
using CallRecording.DAL.Repository;
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
        // GET: api/<MainDataController>
        [HttpGet, Route("api/get/events")]
        public ActionResult<List<Event>> GetEvents()
        {
            var events = new EventRepository();
            if(events == null)
            {
                return NotFound();
            }
            List<Event> data = events.GetList().ToList();
            events.Dispose();
            return data;
        }

        [HttpGet, Route("api/get/events/{id}")]
        public ActionResult<Event> GetEventById(int id)
        {
            var events = new EventRepository();
            if (events == null)
            {
                return NotFound();
            }
            Event data = events.GetById(id);
            events.Dispose();
            return data;
        }

        [HttpGet, Route("api/get/users")]
        public ActionResult<List<User>> GetUsers()
        {
            var users = new  UserRepository();
            if (users == null)
            {
                return NotFound();
            }
            List<User> data = users.GetList().ToList();
            users.Dispose();
            return data;
        }

        [HttpGet, Route("api/get/users/{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var users = new UserRepository();
            if (users == null)
            {
                return NotFound();
            }
            User data = users.GetById(id);
            users.Dispose();
            return data;
        }

        [HttpPost, Route("api/post/newuser/{name}/{passwdhash}/{role}")]
        public ActionResult<User> Post(string name, string passwdhash, string role)
        {
            if(name == "" || passwdhash == "" || role == "")
            {
                return BadRequest();
            }
            User newUser = new User()
            {
                Login = name,
                Password = passwdhash,
                Role = role
            };
            var user = new UserRepository();
            user.Create(newUser);
            user.Dispose();
            return newUser;
        }
    }
}
