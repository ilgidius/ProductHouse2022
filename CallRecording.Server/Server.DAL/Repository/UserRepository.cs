using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Common.Classes.Models.Common;
using Server.Common.Interfaces.Models.Common;
using Server.Common.Interfaces.Models.IUserModel;
using Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DAL.Repository
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly CallRecordingDbContext db;
        private bool disposed = false;

        public UserRepository()
        {
            this.db = new CallRecordingDbContext();
        }

        /*
         * IRepository implementation
         */
        public IEnumerable<User> GetAll()
        {
            return db.Users.AsNoTracking();
        }

        public User? GetById(long id)
        {
            return db.Users.Find(id);
        }

        public void Create(User item)
        {
            db.Users.Add(item);
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
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

        /*
         * IUserRepository implementation
         */
        public IEnumerable<User> GetUserByRole(string role)
        {
            return db.Users.Where(u => u.Role == role).AsQueryable();
        }

        public User? GetUserByName(string login)
        {
            return db.Users.Where(u => u.Login == login).AsQueryable().FirstOrDefault();
        }

        public long GetUserIdByName(string login)
        {
            User? user = GetUserByName(login);
            if (user != null)
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

        public bool UserIsExist(long id)
        {
            User? user = GetById(id);
            return !(user == null);
        }

        public bool UserIsExist(string login)
        {
            User? user = GetUserByName(login);
            return !(user == null);
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
