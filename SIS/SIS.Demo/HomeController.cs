using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using SIS.WebServer.Results;

namespace SIS.Demo
{
    class HomeController
    {
        public IHttpResponse Index(IHttpRequest httpRequest)
        {
            string content = $"<h1>Hello from home page SessionId = {httpRequest.Session.Id}</h1>";

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}
