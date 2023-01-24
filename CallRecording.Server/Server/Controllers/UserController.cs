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
        private readonly ILogger<UserController> _logger;
        private readonly IUserManager _userManager;

        /// <summary>
        /// User controller constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userManager"></param>
        public UserController(ILogger<UserController> logger, IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        /// <summary>
        /// Authorization and receipt of the JWT token
        /// </summary>
        /// <param name="userLogin"></param>
        /// <response code="200">Creates JWT token</response>
        /// <response code="204">User not found</response>
        [AllowAnonymous]
        [HttpPost ("login")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(204)]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            _logger.LogInformation($"Trying to verify '{userLogin.Login}'");
            var user = _userManager.Authentificate(userLogin); //Check whether user exists
            return user == null ? NoContent() : Ok(_userManager.GenerateJwt(user)); //If exists, generate and return JWT
        }

        /// <summary>
        /// Add a new user (administrator only)
        /// </summary>
        /// <param name="newUser"></param>
        /// <response code="200">User was created</response>
        /// <response code="400">The user already exists</response>
        /// <response code="401">The request was not sent by an administrator</response>
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(401)]
        public IActionResult AddNewUser([FromBody] NewUser newUser)
        {
            _logger.LogInformation($"Checking if {newUser.Role} '{newUser.Login}' has already exidted");
            if (!_userManager.IsExist(newUser.Login)) //Check whether user exists
            {
                _userManager.AddNewUser(newUser); //If doesnt exist, add new one
                _logger.LogInformation($"New {newUser.Role} '{newUser.Login}' was created");
                return Ok();
            }
            _logger.LogWarning($"{newUser.Role} '{newUser.Login}' already exists.");
            return BadRequest("User already exists.");
        }

        /// <summary>
        /// Update user information (administrator only)
        /// </summary>
        /// <param name="newUserInfo"></param>
        /// <response code="200">User information was updated</response>
        /// <response code="400">The password equals to previous one</response>
        /// <response code="401">The request was not sent by an administrator</response>
        [Authorize (Roles = "admin")]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(401)]
        public IActionResult UpdatePassword([FromBody] UserLogin newUserInfo)
        {
            _logger.LogInformation($"Checking whether old password maches new for '{newUserInfo.Login}'");
            if (_userManager.IsSame(newUserInfo)) //Check whether old password maches new
            {
                _logger.LogWarning($"Old password mathes new for '{newUserInfo.Login}'");
                return BadRequest("The password cannot be equal to the previously created one.");
            }
            _userManager.UpdatePassword(newUserInfo); //Update password
            _logger.LogInformation($"Password for '{newUserInfo.Login}' was changed");
            return Ok();
        }

        /// <summary>
        /// Delete user by user id (administrator only)
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">If the user existed, it was deleted</response>
        /// <response code="401">The request was not sent by an administrator</response>
        [Authorize (Roles = "admin")]
        [HttpDelete("id/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult DeleteUserById(long id)
        {
            _logger.LogInformation($"Deleting user with id: {id}");
            _userManager.DeleteUser(id); //Deleting user by id
            return Ok();
        }

        /// <summary>
        /// Delete user by username (administrator only)
        /// </summary>
        /// <param name="username"></param>
        /// <response code="200">If the user existed, it was deleted</response>
        /// <response code="401">The request was not sent by an administrator</response>
        [Authorize (Roles = "admin")]
        [HttpDelete("username/{username}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult DeleteUserByName(string username)
        {
            _logger.LogInformation($"Deleting '{username}'");
            _userManager.DeleteUser(username); //Deleting user by username
            return Ok();
        }
    }
}
