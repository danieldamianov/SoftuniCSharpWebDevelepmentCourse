using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRunes.Database;
using IRunes.Database.Models;

namespace IRunes.Services
{
    public class UserService : IUserService
    {
        public UserService()
        {
            this.runesDbContext = new IRunesDbContext();
        }

        private IRunesDbContext runesDbContext;
        public User AddUser(User user)
        {
            user = this.runesDbContext.Users.Add(user).Entity;
            this.runesDbContext.SaveChanges();
            return user;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return this.runesDbContext.Users.SingleOrDefault(u => u.Username == username
            && u.Password == password);
        }

        public bool IsUsernameTaken(string username)
        {
            return this.runesDbContext.Users.Any(u => u.Username == username);
        }
    }
}
