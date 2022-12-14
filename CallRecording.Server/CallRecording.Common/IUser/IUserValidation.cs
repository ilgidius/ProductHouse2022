using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallRecording.Common.IUser
{
    public interface IUserValidation
    {
        bool UserAuthorized(string username, string password);
        string UserRole(string username);
    }
}
