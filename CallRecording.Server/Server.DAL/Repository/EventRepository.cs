using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Server.Common.Classes.Models.EventModels;
using Server.Common.Interfaces.Models.IEventModel;
using Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.Repository
{
    public class EventRepository : IEventRepository<Event, EventFilter>
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
        public IEnumerable<Event> GetEventsForRelevantUserByFilter(EventFilter filter)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Event> GetEventsByFilter(EventFilter filter)
        {
            throw new NotImplementedException();
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
