using Microsoft.EntityFrameworkCore;
using Server.Common.Interfaces.Models.IEventModel;
using Server.DAL.Models;

namespace Server.DAL.Repository
{
    public class EventRepository : IEventRepository<Event>
    {
        private readonly CallRecordingDbContext db;
        private bool disposed = false;

        public EventRepository()
        {
            this.db = new CallRecordingDbContext();
        }

        /*
         * IRepository implementation
         */
        public IEnumerable<Event> GetAll()
        {
            return db.Events.AsNoTracking();
        }

        public Event? GetById(long id)
        {
            return db.Events.Find(id);
        }

        public void Create(Event item)
        {
            db.Events.Add(item);
        }

        public void Update(Event item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            Event? user = db.Events.Find(id);
            if (user != null)
            {
                db.Events.Remove(user);
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        /*
         * IEventRepository implementation
         */
        public IEnumerable<Event> GetEventsForRelevantUser(long userId)
        {
            return db.Events.Where(u => u.UserId == userId).AsQueryable();
        }

        public void DeleteEventsOlderThan(DateTime date)
        {
            IEnumerable<Event> events = db.Events.ToList();
            List<Event> eventsToDelete = new List<Event>();
            foreach (Event e in events)
            {
                if(Convert.ToDateTime(e.AddedTime) < date)
                {
                    eventsToDelete.Add(e);
                }
            }
            db.Events.RemoveRange(eventsToDelete);
        }

        /*
         * Dispose method
         */
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
