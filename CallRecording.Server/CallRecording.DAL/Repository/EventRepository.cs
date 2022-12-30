using CallRecording.Common.IRepository;
using CallRecording.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallRecording.DAL.Repository
{
    public class EventRepository : IRepository<Event>
    {
        private CallRecordingDbContext db;
        public EventRepository()
        {
            this.db = new CallRecordingDbContext();
        }

        public IEnumerable<Event> GetAll()
        {
            return db.Events;
        }

        public Event? GetById(long id)
        {
            return db.Events.Find(id);
        }

        public void Create(Event callEvent)
        {
            db.Events.Add(callEvent);
        }

        public void Update(Event callEvent)
        {
            db.Entry(callEvent).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            Event? callEvent = db.Events.Find(id);
            if (callEvent != null)
            {
                db.Events.Remove(callEvent);
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

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
