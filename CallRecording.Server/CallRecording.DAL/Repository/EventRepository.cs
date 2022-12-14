using CallRecording.Common.Repository;
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

        public IEnumerable<Event> GetList()
        {
            return db.Events;
        }

        public Event GetById(int id)
        {
            return db.Events.Find(Convert.ToInt64(id));
        }

        public void Create(Event callEvent)
        {
            db.Events.Add(callEvent);
        }

        public void Update(Event callEvent)
        {
            db.Entry(callEvent).State = EntityState.Modified;
        }

        public void Delete(Event callEvent)
        {
            int.TryParse(db.Events.Entry(callEvent).Entity.Id.ToString(), out int id);
            if (callEvent != null)
                db.Events.Remove(callEvent);
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
