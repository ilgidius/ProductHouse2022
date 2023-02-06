using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.EventModels;
using Server.Common.Interfaces.Models.IEventModel;

namespace Server.API.Controllers
{
    /// <summary>
    /// Controller for events
    /// </summary>
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IEventManager _eventManager;
        private readonly IEventNotifyService _notifyService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Event controller constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="eventManager"></param>
        /// <param name="notifyService"></param>
        /// <param name="mapper"></param>
        public EventController(ILogger<UserController> logger, IEventManager eventManager, IEventNotifyService notifyService,
            IMapper mapper)
        {
            _logger = logger;
            _eventManager = eventManager;
            _notifyService = notifyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get events fore relevant users by user id (administrator only)
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">List of events for relevant user</response>
        /// <response code="204">Events were not found</response>
        /// <response code="400">The user does not exist</response>
        /// <response code="401">The request was not sent by an administrator</response>
        [Authorize (Roles = "admin")]
        [HttpGet("id/{id}")]
        [ProducesResponseType(typeof(List<EventModel>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult GetEventsByUserId(long id)
        {
            List<EventModel> data = _eventManager.GetEventsForRelevantUser(id);
            if (data == null)
            {
                return BadRequest(); //400
            }
            else if (data.Count == 0)
            {
                return NoContent(); //204
            }
            return Ok(data); //200
        }

        /// <summary>
        /// Get events fore relevant users by username (administrator only)
        /// </summary>
        /// <param name="username"></param>
        /// <response code="200">List of events for relevant user</response>
        /// <response code="204">Events were not found</response>
        /// <response code="400">The user does not exist</response>
        /// <response code="401">The request was not sent by an administrator</response>
        [Authorize (Roles = "admin")]
        [HttpGet("username/{username}")]
        [ProducesResponseType(typeof(List<Common.Classes.Models.Common.EventModel>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult GetEventsByUsername(string username)
        {
            List<Common.Classes.Models.Common.EventModel> data = _eventManager.GetEventsForRelevantUser(username);
            if (data == null)
            {
                return BadRequest();
            }
            else if (data.Count == 0)
            {
                return NotFound();
            }
            return Ok(data);
        }

        /*
        /// <summary>
        /// Add new event fore relevant user by user id (administrator only)
        /// </summary>
        /// <param name="newEvent"></param>
        /// <response code="200">Event was added</response>
        /// <response code="400">The user does not exist</response>
        /// <response code="401">The request was not sent by an administrator</response>
        [Authorize (Roles = "admin")]
        [HttpPost("id")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult AddNewEventWithUserId([FromBody] NewEventWithUserId newEvent)
        {
            if (_eventManager.AddNewEventById(newEvent))
            {
                _notifyService.AddNotification(_mapper.Map<EventModel>(newEvent));
                return Ok();
            }
            return BadRequest();
        }
        */

        /// <summary>
        /// Add new event fore relevant user by username (administrator only)
        /// </summary>
        /// <param name="newEvent"></param>
        /// <response code="200">List of events for relevant user</response>
        /// <response code="400">The user does not exist</response>
        /// <response code="401">The request was not sent by an administrator</response>
        [Authorize (Roles = "admin")]
        [HttpPost("username")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult AddNewEventWithUsername([FromBody] NewEventWithUsername newEvent)
        {
            if (_eventManager.AddNewEventByUsername(newEvent))
            {
                _notifyService.AddNotification(_mapper.Map<EventModel>(newEvent));
                return Ok();
            }
            return BadRequest();
        }
    }
}
