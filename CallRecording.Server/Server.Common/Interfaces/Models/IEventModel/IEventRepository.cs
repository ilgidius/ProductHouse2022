using Server.Common.Interfaces.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Common.Interfaces.Models.IEventModel
{
    public interface IEventRepository<T, TFilter> : IRepository<T>
        where T : class
        where TFilter : class
    {
        IEnumerable<T> GetEventsForRelevantUser(long userId);
        IEnumerable<T> GetEventsForRelevantUserByFilter(TFilter filter);
        IEnumerable<T> GetEventsByFilter(TFilter filter);
    }
}
