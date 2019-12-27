using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.Results
{
    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpResponseStatusCode httpResponseStatusCode, string contentType = @"text/html charset=utf-8")
            : base(httpResponseStatusCode)
        {
            Headers.AddHeader(new HTTP.Headers.HttpHeader("Content-Type", contentType));
            Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
