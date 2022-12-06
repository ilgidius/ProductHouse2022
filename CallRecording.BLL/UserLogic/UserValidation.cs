using CallRecording.Common.IUser;
using CallRecording.Common.Repository;
using CallRecording.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallRecording.BLL.UserLogic
{
    public class UserValidation : IUserValidation
    {
        private IRepository<User> _repository;
        public UserValidation(IRepository<User> repository)
        {
            this._repository = repository;
        }

        public bool UserAuthorized(string username, string password)
        {

            return true;
        }
        public string UserRole(string username)
        {
            return "";
        }

    }
}
