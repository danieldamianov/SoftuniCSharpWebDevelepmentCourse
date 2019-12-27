using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
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

        public Controller()
        {
            this.ViewData = new Dictionary<string, string>();
        }
        protected IHttpResponse View([CallerMemberName] string name = null)
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

        protected bool IsLogedIn(IHttpRequest httpRequest)
        {
            return httpRequest.Session.ContainsParameter("username");
        }

        protected IHttpResponse Redirect(string location)
        {
            return new RedirectResult(location);
        }

        
    }
}
