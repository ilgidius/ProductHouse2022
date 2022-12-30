using CallRecording.Common.IUser;
using CallRecording.Common.IRepository;
using CallRecording.DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace CallRecording.BLL.UserLogic
{
    public class UserValidation : IUserValidation<User, UserLogin>
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly IConfiguration _config;
        public UserValidation(IUserRepository<User> userRepository, IConfiguration config)
        {
            _config = config;
            _userRepository = userRepository;
        }

        public string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Issuer"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Login),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tocken = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], audience: _config["Jwt:Audience"],
                claims: claims, expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(tocken);
        }

        public User? Authentificate(UserLogin userLogin)
        {
            User? authentificate = _userRepository.GetUserByName(userLogin.Login);
            if (authentificate == null)
            {
                return null;
            }
            else if (authentificate.Password != userLogin.Password)
            {
                return null;
            }
            return authentificate;
        }
    }
}
