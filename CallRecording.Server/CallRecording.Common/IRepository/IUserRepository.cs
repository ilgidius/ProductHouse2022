namespace CallRecording.Common.IRepository
{
    public interface IUserRepository<T>
        where T : class
    {
        IEnumerable<T> GetUserByRole(string role);
        T? GetUserByName(string login);
        long GetUserIdByName(string login);
        void DeleteUserByName(string login);
    }
}
