using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.BLL.Background;
using System.ComponentModel.DataAnnotations;

namespace Server.API.Controllers
{
    /// <summary>
    /// Controller for Background Service
    /// </summary>
    [Route("api/background")]
    [ApiController]
    public class Background : ControllerBase
    {
        private readonly ILogger<WBSController> _log;

        /// <summary>
        /// Background Service controller constructor
        /// </summary>
        /// <param name="logger"></param>
        public Background(ILogger<WBSController> logger)
        {
            _log = logger;
        }

        /// <summary>
        /// Changes the state of the periodic service (administrator only)
        /// </summary>
        /// <param name="state"></param>
        /// <response code="200">State succesfully changed</response>
        /// <response code="401">The request was not sent by an administrator</response>
        /// <response code="500">Internal server error</response>
        [Authorize (Roles = "admin")]
        [HttpPatch]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult Patch([FromQuery] bool state)
        {
            try
            {
                PeriodicHostedService.IsEnabled = state;
                _log.LogInformation($"State of periodic service was changed to {state}");
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
