using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Results
{
    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpResponseStatusCode httpResponseStatusCode, string contentType = @"text/html charset=utf-8")
            : base(httpResponseStatusCode)
        {
            this.Headers.AddHeader(new HTTP.Headers.HttpHeader("Content-Type", contentType));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
