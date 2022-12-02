using CallRecording.Models;
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
        [HttpGet, Route("api/get")]
        public IEnumerable<MainData> Get()
        {
            MainData someData = new MainData()
            {
                AddedTime= DateTime.Now,
                SentTime= DateTime.Now,
                EventType = "START",
                BussinesData = new List<BusinessData> { new BusinessData() { Key = "test", Value= "test" } }
            };
            List<MainData> result = new List<MainData>();
            result.Add(someData);
            return result;
        }
        [HttpPost, Route("api/post/newuser/{name}/{passwdhash}/{role}")]
        public void Post(string name, string passwdhash, string role)
        {
            User newUser = new User()
            {
                Login = name,
                Password = passwdhash,
                Role = role
            };

            using(var db = new CallRecordingDbContext())
            {
                db.Add(newUser);
                db.SaveChanges();
            }
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "value");
            //response.Content = new StringContent("hello", Encoding.Unicode);
            //response.Headers.CacheControl = new CacheControlHeaderValue()
            //{
            //    MaxAge = TimeSpan.FromMinutes(20)
            //};
            //return response;
        }
        /*
        // GET api/<MainDataController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MainDataController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MainDataController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MainDataController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
