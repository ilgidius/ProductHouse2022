using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.API.Controllers
{
    /// <summary>
    /// Controller for chunked responces
    /// </summary>
    [Route("api/chunked")]
    [ApiController]
    public class ChunckedController : ControllerBase
    {
        private readonly ILogger<ChunckedController> _log;

        /// <summary>
        /// Chunked controller constructor
        /// </summary>
        /// <param name="log"></param>
        public ChunckedController(ILogger<ChunckedController> log)
        {
            _log = log;
        }

        /// <summary>
        /// Get chunked responce
        /// </summary>
        /// <response code="200">Successfully transfering data</response>
        /// <response code="401">The request was not sent by an authorized user</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task ChunkedStream()
        {
            try
            {
                var response = HttpContext.Response;
                response.StatusCode = 200;
                response.ContentType = "text/event-stream";

                for (var i = 0; i < 10; ++i)
                {
                    await response.WriteAsync($"data: test {i}\n\n");
                    //response.BodyWriter.FlushAsync();
                    await response.Body.FlushAsync();
                    await Task.Delay(1000);
                }
                await response.WriteAsync("data:\n\n");
                await response.Body.FlushAsync();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
