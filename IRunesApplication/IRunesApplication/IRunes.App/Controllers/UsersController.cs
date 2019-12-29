using IRunes.Database;
using IRunes.Database.Models;

using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.HttpAttributes;
using SIS.MvcFramework.Results;

using System;
using System.Linq;
using System.Web;

namespace IRunes.App.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Login()
        {
            return this.View();
        }

        public ActionResult Register()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Register")]
        public ActionResult HandleRegistration()
        {
            string username = (string)this.Request.FormData["username"];
            string password = (string)this.Request.FormData["password"];
            string confirmPassword = (string)this.Request.FormData["confirmPassword"];

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


                runesDbContext.SaveChanges();

                this.SignIn(user.Id, user.Username, user.Email);

                return this.Redirect("/Home/HomePage");
            }
        }

        [HttpGet(ActionName = "Logout")]
        public ActionResult HandleLoggingOut()
        {
            this.SignOut();
            return this.Redirect("/");
        }

        [HttpPost(ActionName = "Login")]
        public ActionResult HandleLogingIn()
        {
            string password = (string)this.Request.FormData["password"];
            string username = (string)this.Request.FormData["username"];

            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {

                User user = runesDbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user == null)
                {
                    return this.Redirect("/Users/Login");
                }


                runesDbContext.SaveChanges();

                this.SignIn(user.Id,user.Username,user.Email);

                return this.Redirect("/Home/HomePage");
            }
        }


    }
}
