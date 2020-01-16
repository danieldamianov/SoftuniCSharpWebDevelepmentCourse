using IRunes.App.ViewModels;
using IRunes.Database;
using SIS.HTTP.Identity;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.HttpAttributes;
using SIS.MvcFramework.Results;

using System.IO;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {

        //public ActionResult Do()
        //{
        //    Person user = new Person() { Age = 3 ,Name = "adsd"};
        //    return new FileResult(File.ReadAllBytes(@"D:\test.txt"), SIS.HTTP.Enums.HttpResponseStatusCode.Ok);
        //    //return this.Json(user);
        //}


        [HttpGet(Url = "/")]
        public ActionResult HomePageSlash()
        {
            return HomePage();
        }
        public ActionResult HomePage()
        {
            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {
                if (this.IsLogedIn())
                {
                    return this.View(name: "LogedIn",model:
                        new HomeLogedInViewModel() { Username = ((Principal)this.Request.Session.GetParameter("principal")).Username });
                }
                return this.View(name: "LogedOut");
            }
        }
    }
}
