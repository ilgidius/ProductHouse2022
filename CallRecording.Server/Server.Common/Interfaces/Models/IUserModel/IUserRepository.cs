using Server.Common.Interfaces.Models.Common;

namespace Server.Common.Interfaces.Models.IUserModel
{
    public interface IUserRepository<T> : IRepository<T>
        where T : class
    {
        IEnumerable<T> GetUserByRole(string role);
        T? GetUserByName(string login);
        long GetUserIdByName(string login);
        void DeleteUserByName(string login);
        bool UserIsExist(long id);
        bool UserIsExist(string login);
    }
}
