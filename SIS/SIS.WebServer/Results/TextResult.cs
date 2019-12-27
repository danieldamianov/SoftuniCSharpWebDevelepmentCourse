using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Enums;
using SIS.HTTP.Responses;

namespace SIS.MvcFramework.Results
{
    class TextResult : HttpResponse
    {
        public TextResult(string content, HttpResponseStatusCode httpResponseStatusCode, string contentType = @"text/plain charset=utf-8")
            : base(httpResponseStatusCode)
        {
            Headers.AddHeader(new HTTP.Headers.HttpHeader("Content-Type", contentType));
            Content = Encoding.UTF8.GetBytes(content);
        }

        public TextResult(byte[] content, HttpResponseStatusCode httpResponseStatusCode, string contentType = @"text/plain charset=utf-8")
            : base(httpResponseStatusCode)
        {
            Content = content;
            Headers.AddHeader(new HTTP.Headers.HttpHeader("Content-Type", contentType));
        }
    }
}
