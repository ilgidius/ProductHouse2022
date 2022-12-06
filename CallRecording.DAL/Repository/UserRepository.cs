using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CallRecording.DAL;
using CallRecording.DAL.Models;
using CallRecording.Common.Repository;
using Microsoft.EntityFrameworkCore;

namespace CallRecording.DAL.Repository
{
    public class UserRepository : IRepository<User>
    {
        private CallRecordingDbContext db;
        public UserRepository()
        {
            this.db = new CallRecordingDbContext();
        }

        public IEnumerable<User> GetList()
        {
            return db.Users;
        }

        public User GetById(int id)
        {
            return db.Users.Find(id);
        }

        public void Create(User user)
        {
            db.Users.Add(user);
        }

        public void Update(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public void Delete(User user)
        {
            int.TryParse(db.Users.Entry(user).Entity.Id.ToString(), out int id);
            if (user != null)
                db.Users.Remove(user);
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
