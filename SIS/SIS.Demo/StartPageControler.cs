using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Demo
{
    class StartPageControler
    {
        public IHttpResponse LoadStartPage(IHttpRequest httpRequest)
        {
            string content = $"<h1>Hello from home startPage SessionId = {httpRequest.Session.Id}</h1>" +
                $"<a href=\"register\">Register</a>" +
                $"<a href=\"login\">Login</a>";

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}
