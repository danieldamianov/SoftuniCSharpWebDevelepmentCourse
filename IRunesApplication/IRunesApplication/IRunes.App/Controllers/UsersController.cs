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
            //bool rememberMe = (bool)httpRequest.FormData["remember"];

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
                    Username = username
                };
                runesDbContext.Users.Add(user);

                httpRequest.Session.AddParameter("username", username);
                httpRequest.Session.AddParameter("userId", user.Id);
                //if (runesDbContext.Sessions.Find(httpRequest.Session.Id) == null)
                //{
                //    runesDbContext.Sessions.Add(new Database.Models.Session() { Id = httpRequest.Session.Id, UserId = user.Id });
                //}

                runesDbContext.SaveChanges();

                return new RedirectResult("/Home/Index");
            }
        }

        public IHttpResponse HandleLoggingOut(IHttpRequest httpRequest)
        {
            httpRequest.Session.ClearParameters();
            //this.ViewData.Clear();
            return this.Redirect("/");
            //using (IRunesDbContext runesDbContext = new IRunesDbContext())
            //{
            //    runesDbContext.Sessions.Remove(runesDbContext.Sessions.Find(httpRequest.Session.Id));
            //    runesDbContext.SaveChanges();
            //}
            //IHttpResponse httpResponse = new RedirectResult("/");
            //return httpResponse;
            ////httpRequest.Session = null;
        }
        public IHttpResponse HandleLogingIn(IHttpRequest httpRequest)
        {
            string username = (string)httpRequest.FormData["username"];
            string password = (string)httpRequest.FormData["password"];
            //string rememberMe = httpRequest.FormData.ContainsKey("remember") ? (string)httpRequest.FormData["remember"] : "";

            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {

                User user = runesDbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user == null)
                {
                    return this.Redirect("/Users/Login");
                }

                httpRequest.Session.AddParameter("username", username);
                httpRequest.Session.AddParameter("userId", user.Id);
                //Database.Models.User user = new Database.Models.User
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    Password = password,
                //    Username = username
                //};
                //runesDbContext.Users.Add(user);

                //if (runesDbContext.Sessions.Find(httpRequest.Session.Id) == null)
                //{
                //    runesDbContext.Sessions.Add(new Database.Models.Session() { Id = httpRequest.Session.Id, UserId = user.Id });
                //}
                //else
                //{
                //    runesDbContext.Sessions.Find(httpRequest.Session.Id).UserId = user.Id;
                //}

                runesDbContext.SaveChanges();


                //RedirectResult redirectResult = new RedirectResult("/Home/Index");
                return this.Redirect("/Home/Index");
            }
        }


    }
}
