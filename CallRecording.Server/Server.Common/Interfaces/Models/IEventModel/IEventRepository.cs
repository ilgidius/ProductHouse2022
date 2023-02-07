using Server.Common.Interfaces.Models.Common;

namespace Server.Common.Interfaces.Models.IEventModel
{
    public interface IEventRepository<T> : IRepository<T>
        where T : class
    {
        IEnumerable<T> GetEventsForRelevantUser(long userId);
        void DeleteEventsOlderThan(DateTime date);
    }
}
