using IRunes.Database;
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
    public class HomeController : BaseController
    {
        public IHttpResponse HomePage(IHttpRequest httpRequest)
        {
            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {
                if (this.IsLogedIn(httpRequest))
                {
                    this.ViewData.Clear();
                    this.ViewData.Add("@Username", (string)httpRequest.Session.GetParameter("username"));
                    return this.LogedIn();
                }
                return this.LogedOut();
            }
        }

        private IHttpResponse LogedOut()
        {
             return this.View();
        }

        private IHttpResponse LogedIn()
        {
            return this.View();
        }
    }
}
