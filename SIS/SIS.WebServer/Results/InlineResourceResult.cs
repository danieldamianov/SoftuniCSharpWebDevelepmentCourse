using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Results
{
    public class InlineResourceResult : HttpResponse
    {
        public InlineResourceResult(byte[] content,HttpResponseStatusCode httpResponseStatusCode)
            :base(httpResponseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Content = content;
        }
    }
}
