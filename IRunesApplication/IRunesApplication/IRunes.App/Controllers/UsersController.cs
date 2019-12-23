using IRunes.Database;
using IRunes.Database.Models;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace IRunes.App.Controllers
{
    class UsersController : BaseController
    {
        public IHttpResponse Login(IHttpRequest httpRequest)
        {
            return this.View();
        }

        public IHttpResponse Register(IHttpRequest httpRequest)
        {
            return this.View();
        }

        public IHttpResponse HandleRegistration(IHttpRequest httpRequest)
        {
            string username = (string)httpRequest.FormData["username"];
            string password = (string)httpRequest.FormData["password"];
            string confirmPassword = (string)httpRequest.FormData["confirmPassword"];

            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {

                if (password != confirmPassword || runesDbContext.Users.Any(u => u.Username == username))
                {
                    return this.Redirect("/Users/Register");
                }

                Database.Models.User user = new Database.Models.User
                {
                    Id = Guid.NewGuid().ToString(),
                    Password = password,
                    Username = HttpUtility.UrlDecode(username)
                };
                runesDbContext.Users.Add(user);

                httpRequest.Session.AddParameter("username", username);
                httpRequest.Session.AddParameter("userId", user.Id);

                runesDbContext.SaveChanges();

                return new RedirectResult("/Home/Index");
            }
        }

        public IHttpResponse HandleLoggingOut(IHttpRequest httpRequest)
        {
            httpRequest.Session.ClearParameters();

            return this.Redirect("/");
        }
        public IHttpResponse HandleLogingIn(IHttpRequest httpRequest)
        {
            string username = (string)httpRequest.FormData["username"];
            string password = (string)httpRequest.FormData["password"];

            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {

                User user = runesDbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user == null)
                {
                    return this.Redirect("/Users/Login");
                }

                httpRequest.Session.AddParameter("username", username);
                httpRequest.Session.AddParameter("userId", user.Id);

                runesDbContext.SaveChanges();

                return this.Redirect("/Home/Index");
            }
        }


    }
}
