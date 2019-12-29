using SIS.HTTP.Identity;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework.Extensions;
using SIS.MvcFramework.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SIS.MvcFramework
{
    public class Controller
    {
        protected Dictionary<string, string> ViewData;

        protected internal Principal User => this.Request.Session.ContainsParameter("principal")
            ? (Principal)this.Request.Session.GetParameter("principal")
            : null;

        protected internal IHttpRequest Request { get; set; }

        public Controller()
        {
            this.ViewData = new Dictionary<string, string>();
        }
        protected ActionResult View([CallerMemberName] string name = null)
        {
            string folderName = this.GetType().Name.Replace("Controller", string.Empty);

            string viewPath = $"Views/{folderName}/{name}.html";

            string path = $"./{viewPath}";

            string htmlAsString = File.ReadAllText(path);

            htmlAsString = this.FillWithData(htmlAsString);

            return new HtmlResult(htmlAsString, SIS.HTTP.Enums.HttpResponseStatusCode.Ok);
        }

        private string FillWithData(string htmlAsString)
        {
            foreach (var item in this.ViewData)
            {
                htmlAsString = htmlAsString.Replace(item.Key,item.Value);
            }

            return htmlAsString;
        }

        protected bool IsLogedIn()
        {
            return this.User != null;
        }

        protected void SignIn(string id,string username,string email)
        {
            this.Request.Session.AddParameter
                (
                "principal",
                new Principal()
                {
                    Id = id,
                    Username = username,
                    Email = email
                }
                );
        }

        protected void SignOut()
        {
            this.Request.Session.ClearParameters();
        }

        protected ActionResult Redirect(string location)
        {
            return new RedirectResult(location);
        }

        protected ActionResult Xml(object param)
        {
            return new XmlResult(param.ToXml(),HTTP.Enums.HttpResponseStatusCode.Ok);
        }

        protected ActionResult Json(object param)
        {
            return new JsonResult(param.ToJson(), HTTP.Enums.HttpResponseStatusCode.Ok);
        }

        protected ActionResult ReturnFile(byte[] content)
        {
            return new FileResult(content, HTTP.Enums.HttpResponseStatusCode.Ok);
        }

        protected ActionResult NotFound(string message = "")
        {
            return new NotFoundResult(message, HTTP.Enums.HttpResponseStatusCode.Ok);
        }

        
    }
}
