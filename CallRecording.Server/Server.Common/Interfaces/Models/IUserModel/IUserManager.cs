using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.UserModels;

namespace Server.Common.Interfaces.Models.IUserModel
{
    public interface IUserManager
    {
        bool IsExist(string login);
        void AddNewUser(NewUser newUser);
        void DeleteUser(long id);
        void DeleteUser(string username);
        bool IsSame(UserLogin user);
        void UpdatePassword(UserLogin user);
        UserModel? Authentificate(UserLogin userLogin);
        string GenerateJwt(UserModel user);
    }
}
