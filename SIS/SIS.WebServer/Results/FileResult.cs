using System;
using System.Collections.Generic;
using System.Text;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.MvcFramework.Results
{
     public class FileResult : ActionResult
    {
        public FileResult(byte[] content, HttpResponseStatusCode httpResponseStatusCode) : base(httpResponseStatusCode)
        {
            this.AddHeader(new HTTP.Headers.HttpHeader(HttpHeader.ContentLength,content.Length.ToString()));
            this.AddHeader(new HTTP.Headers.HttpHeader(HttpHeader.ContentDisposition,"attachment"));
            this.Content = content;
        }
    }
}
