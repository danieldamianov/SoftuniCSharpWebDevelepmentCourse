using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Responses
{
    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; set; }

        IHttpHeaderCollection Headers { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader httpHeader);

        void AddCookie(HttpCookie httpCookie);

        byte[] GetBytes();
    }
}
