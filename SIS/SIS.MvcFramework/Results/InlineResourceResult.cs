using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.Results
{
    public class InlineResourceResult : ActionResult
    {
        public InlineResourceResult(byte[] content, HttpResponseStatusCode httpResponseStatusCode)
            : base(httpResponseStatusCode)
        {
            Headers.AddHeader(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            Headers.AddHeader(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            Content = content;
        }
    }
}
