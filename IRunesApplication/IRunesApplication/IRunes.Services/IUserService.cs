using IRunes.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Services
{
    public interface IUserService
    {
        User AddUser(User user);

        User GetUserByUsernameAndPassword(string username, string password);
        bool IsUsernameTaken(string username);
    }
}
