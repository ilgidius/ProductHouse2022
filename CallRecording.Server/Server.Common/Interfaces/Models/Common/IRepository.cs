namespace Server.Common.Interfaces.Models.Common
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(long id);
        void Create(T item);
        void Update(T item);
        void Delete(long id);
        void Save();
    }
}
