using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Enums;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    class TextResult : HttpResponse
    {
        public TextResult(string content,HttpResponseStatusCode httpResponseStatusCode,string contentType = @"text/plain charset=utf-8")
            :base(httpResponseStatusCode)
        {
            this.Headers.AddHeader(new HTTP.Headers.HttpHeader("Content-Type",contentType));
            this.Content = Encoding.UTF8.GetBytes(content);
        }

        public TextResult(byte[] content, HttpResponseStatusCode httpResponseStatusCode, string contentType = @"text/plain charset=utf-8")
            : base(httpResponseStatusCode)
        {
            this.Content = content;
            this.Headers.AddHeader(new HTTP.Headers.HttpHeader("Content-Type", contentType));
        }
    }
}
