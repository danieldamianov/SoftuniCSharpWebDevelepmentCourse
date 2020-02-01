using IRunes.Database.Models;
using IRunes.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.HttpAttributes;
using SIS.MvcFramework.Results;
using System;
using System.Web;

namespace IRunes.App.Controllers
{
    public class UsersController : Controller
    {
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        IUserService userService;

        public ActionResult Login()
        {
            return this.View();
        }

        public ActionResult Register()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Register")]
        public ActionResult HandleRegistration(string username,string password,string confirmPassword)
        {
            if (password != confirmPassword || this.userService.IsUsernameTaken(username))
            {
                return this.Redirect("/Users/Register");
            }

            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Password = password,
                Username = HttpUtility.UrlDecode(username)
            };

            this.userService.AddUser(user);

            this.SignIn(user.Id, user.Username, user.Email);

            return this.Redirect("/Home/HomePage");
        }

        [HttpGet(ActionName = "Logout")]
        public ActionResult HandleLoggingOut()
        {
            this.SignOut();
            return this.Redirect("/");
        }

        [HttpPost(ActionName = "Login")]
        public ActionResult HandleLogingIn(string password,string username)
        {
            User user = this.userService.GetUserByUsernameAndPassword(username, password);

            if (user == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(user.Id, user.Username, user.Email);

            return this.Redirect("/Home/HomePage");
        }
    }
}
