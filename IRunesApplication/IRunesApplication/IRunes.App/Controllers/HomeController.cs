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
    public class HomeController
    {
        

        public IHttpResponse HomePage(IHttpRequest httpRequest)
        {
            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {
                if (runesDbContext.Sessions.Find(httpRequest.Session.Id) != null)
                {
                    Database.Models.Session session = runesDbContext.Sessions.Find(httpRequest.Session.Id);
                    string username = runesDbContext.Users.Find(session.UserId).Username;
                    return new HtmlResult(File.ReadAllText("Controllers/HomePageLogedIn.html").Replace("@Username",username), SIS.HTTP.Enums.HttpResponseStatusCode.Ok);
                }
                return new HtmlResult(File.ReadAllText("Controllers/HomePageLogedOut.html"), SIS.HTTP.Enums.HttpResponseStatusCode.Ok); 
            }
        }
    }
}
