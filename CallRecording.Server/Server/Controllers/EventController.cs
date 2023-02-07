using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.EventModels;
using Server.Common.Interfaces.Models.IEventModel;
using Server.Common.Interfaces.Models.INotification;

namespace Server.API.Controllers
{
    /// <summary>
    /// Controller for events
    /// </summary>
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _log;
        private readonly IEventManager _eventManager;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Event controller constructor
        /// </summary>
        /// <param name="log"></param>
        /// <param name="eventManager"></param>
        /// <param name="notificationService"></param>
        /// <param name="mapper"></param>
        public EventController(ILogger<EventController> log, IEventManager eventManager,
            INotificationService notificationService, IMapper mapper)
        {
            _log = log;
            _eventManager = eventManager;
            _notificationService = notificationService;
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
        /// <response code="500">Internal server error</response>
        [Authorize (Roles = "admin")]
        [HttpGet("id/{id}")]
        [ProducesResponseType(typeof(List<EventModel>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult GetEventsByUserId(long id)
        {
            try
            {
                List<EventModel> data = _eventManager.GetEventsForRelevantUser(id);
                if (data == null)
                {
                    return StatusCode(400); //400
                }
                else if (data.Count == 0)
                {
                    return StatusCode(204); //204
                }
                return StatusCode(200, data); //200
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get events fore relevant users by username (administrator only)
        /// </summary>
        /// <param name="username"></param>
        /// <response code="200">List of events for relevant user</response>
        /// <response code="204">Events were not found</response>
        /// <response code="400">The user does not exist</response>
        /// <response code="401">The request was not sent by an administrator</response>
        /// <response code="500">Internal server error</response>
        [Authorize (Roles = "admin")]
        [HttpGet("username/{username}")]
        [ProducesResponseType(typeof(List<EventModel>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult GetEventsByUsername(string username)
        {
            try
            {
                List<EventModel> data = _eventManager.GetEventsForRelevantUser(username);
                if (data == null)
                {
                    return StatusCode(400);
                }
                else if (data.Count == 0)
                {
                    return StatusCode(204);
                }
                return StatusCode(200, data);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Add new event fore relevant user by username (administrator only)
        /// </summary>
        /// <param name="newEvent"></param>
        /// <response code="200">List of events for relevant user</response>
        /// <response code="400">The user does not exist</response>
        /// <response code="401">The request was not sent by an administrator</response>
        /// <response code="500">Internal server error</response>
        [Authorize (Roles = "admin")]
        [HttpPost("username")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult AddNewEventWithUsername([FromBody] NewEventWithUsername newEvent)
        {
            try
            {
                if (_eventManager.AddNewEventByUsername(newEvent))
                {
                    _notificationService.AddEventIntoQueue(_mapper.Map<EventModel>(newEvent));
                    return StatusCode(200);
                }
                return StatusCode(400);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
