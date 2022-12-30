using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CallRecording.DAL;
using CallRecording.DAL.Models;
using CallRecording.Common.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CallRecording.DAL.Repository
{
    public class UserRepository : IRepository<User>, IUserRepository<User>
    {
        private readonly CallRecordingDbContext db;
        public UserRepository()
        {
            this.db = new CallRecordingDbContext();
        }

        /*
         * Implementation of IRepository<T> 
         */
        public IEnumerable<User> GetAll()
        {
            return db.Users;
        }

        public User? GetById(long id)
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

        public void Delete(long id)
        {
            User? user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user);
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

        /*
         * Implementation of IUserRepository<T> 
         */
        public IEnumerable<User> GetUserByRole(string role)
        {
            /* LINQ query
            var query = from u in GetAll()
                        where u.Role == role
                        orderby u.Login
                        select u;
            */
            return db.Users.Where(u => u.Role == role);
        }

        public User? GetUserByName(string login)
        {
            return db.Users.Where(u => u.Login == login).FirstOrDefault();
        }

        public long GetUserIdByName(string login)
        {
            User? user = GetUserByName(login);
            if(user != null)
            {
                return user.Id;
            }
            throw new Exception("User not found");
        }

        public void DeleteUserByName(string login)
        {
            User? user = GetUserByName(login);
            if (user != null)
            {
                db.Users.Remove(user);
            }
        }
    }
}
