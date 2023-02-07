using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.UserModels;
using Server.Common.Interfaces.Models.IUserModel;
using Server.DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Server.BLL.Managers.UserManager
{
    public class UserManager : IUserManager
    {
        private readonly ILogger<UserManager> _log;
        private readonly IConfiguration _config;
        private readonly IUserRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserManager(ILogger<UserManager> logger,
            IConfiguration config, IUserRepository<User> userRepository, IMapper mapper)
        {
            _log = logger;
            _config = config;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public bool IsExist(string login)
        {
            _log.LogInformation($"Trying to verify whether '{login}' exists");
            return _userRepository.UserIsExist(login);
        }

        public void AddNewUser(NewUser newUser)
        {
            User addNewUser = _mapper.Map<User>(newUser);
            addNewUser.Password = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.Default.GetBytes(addNewUser.Password)));
            _log.LogInformation($"Try to add new {addNewUser.Role} '{addNewUser.Login}'");
            _userRepository.Create(addNewUser);
            _userRepository.Save();
        }
        public void DeleteUser(long id)
        {
            _log.LogInformation($"Try to delete admin/user with id {id}");
            _userRepository.Delete(id);
            _userRepository.Save();
        }

        public void DeleteUser(string username)
        {
            _log.LogInformation($"Try to delete admin/user with username {username}");
            _userRepository.DeleteUserByName(username);
            _userRepository.Save();
        }

        public bool IsSame(UserLogin user)
        {
            _log.LogInformation($"Try to verify whether old password mathces new one for '{user.Login}'");
            User? curr = _userRepository.GetUserByName(user.Login);
            return curr.Password == Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.Default.GetBytes(user.Password)));
        }

        public void UpdatePassword(UserLogin user)
        {
            User? curr = _userRepository.GetUserByName(user.Login);
            curr.Password = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.Default.GetBytes(user.Password)));
            _log.LogInformation($"Try to update password for '{user.Login}'");
            _userRepository.Update(curr);
            _userRepository.Save();
        }

        public UserModel? Authentificate(UserLogin userLogin)
        {
            User? authentificate = _userRepository.GetUserByName(userLogin.Login ?? string.Empty);
            if (authentificate == null)
            {
                _log.LogWarning($"User '{userLogin.Login}' not found");
                return null;
            }
            /* Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(userLogin.Password);
            //Get the salt
            //byte[] salt = new byte[16];
            //Array.Copy(hashBytes, 0, salt, 0, 16);
            // Compute the hash on the password the user entered 
            //var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            //byte[] hash = pbkdf2.GetBytes(20);
            // Compare the results
            for (int i = 0; i < 20; i++)
            if (hashBytes[i + 16] != hash[i])
            throw new UnauthorizedAccessException();*/
            else if (authentificate.Password != Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.Default.GetBytes(userLogin.Password))))
            {
                _log.LogWarning($"Password for '{userLogin.Login}' does not match");
                return null;
            }
            _log.LogInformation($"User '{userLogin.Login}' was authorized");
            UserModel authentificatedUser = _mapper.Map<UserModel>(authentificate);
            return authentificatedUser;
        }

        public string GenerateJwt(UserModel user)
        {
            _log.LogInformation($"Creating JWT for '{user.Login}'");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Login),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tocken = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], audience: _config["Jwt:Audience"],
                claims: claims, expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);

            _log.LogInformation($"JWT for '{user.Login}' was generated");

            return new JwtSecurityTokenHandler().WriteToken(tocken);
        }
    }
}
