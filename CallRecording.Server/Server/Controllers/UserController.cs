using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.UserModels;
using Server.Common.Interfaces.Models.IUserModel;

namespace Server.API.Controllers
{
    /// <summary>
    /// Controller for users
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _log;
        private readonly IUserManager _userManager;

        /// <summary>
        /// User controller constructor
        /// </summary>
        /// <param name="log"></param>
        /// <param name="userManager"></param>
        public UserController(ILogger<UserController> log, IUserManager userManager)
        {
            _log = log;
            _userManager = userManager;
        }

        /// <summary>
        /// Authorization and receipt of the JWT token
        /// </summary>
        /// <param name="userLogin"></param>
        /// <response code="200">Creates JWT token</response>
        /// <response code="204">User not found</response>
        /// <response code="500">Internal server error</response>
        [AllowAnonymous]
        [HttpPost ("login")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            try
            {
                var user = _userManager.Authentificate(userLogin); //Check whether user exists
                return user == null ? StatusCode(204) : StatusCode(200, _userManager.GenerateJwt(user)); //If exists, generate and return JWT
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Add a new user (administrator only)
        /// </summary>
        /// <param name="newUser"></param>
        /// <response code="200">User was created</response>
        /// <response code="400">The user already exists</response>
        /// <response code="401">The request was not sent by an administrator</response>
        /// <response code="500">Internal server error</response>
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult AddNewUser([FromBody] NewUser newUser)
        {
            try
            {
                if (!_userManager.IsExist(newUser.Login)) //Check whether user exists
                {
                    _userManager.AddNewUser(newUser); //If doesnt exist, add new one
                    return StatusCode(200);
                }
                return StatusCode(400, "User already exists.");
            }
            catch(Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Update user information (administrator only)
        /// </summary>
        /// <param name="newUserInfo"></param>
        /// <response code="200">User information was updated</response>
        /// <response code="400">The password equals to previous one</response>
        /// <response code="401">The request was not sent by an administrator</response>
        /// <response code="500">Internal server error</response>
        [Authorize (Roles = "admin")]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult UpdatePassword([FromBody] UserLogin newUserInfo)
        {
            try
            {
                if (_userManager.IsSame(newUserInfo)) //Check whether old password maches new
                {
                    return StatusCode(400, "The password cannot be equal to the previously created one.");
                }
                _userManager.UpdatePassword(newUserInfo); //Update password
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Delete user by user id (administrator only)
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">If the user existed, it was deleted</response>
        /// <response code="401">The request was not sent by an administrator</response>
        /// <response code="500">Internal server error</response>
        [Authorize (Roles = "admin")]
        [HttpDelete("id/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult DeleteUserById(long id)
        {
            try
            {
                _userManager.DeleteUser(id); //Deleting user by id
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Delete user by username (administrator only)
        /// </summary>
        /// <param name="username"></param>
        /// <response code="200">If the user existed, it was deleted</response>
        /// <response code="401">The request was not sent by an administrator</response>
        /// <response code="500">Internal server error</response>
        [Authorize (Roles = "admin")]
        [HttpDelete("username/{username}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult DeleteUserByName(string username)
        {
            try
            {
                _userManager.DeleteUser(username); //Deleting user by username
                return StatusCode(200);
            }
            catch(Exception ex)
            {
                _log.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
