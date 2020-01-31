using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Sessions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Requests
{
    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        Dictionary<string, ISet<string>> FormData { get; }

        Dictionary<string, ISet<string>> QueryData { get; }

        HttpRequestMethod RequestMethod { get; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }
        IHttpSession Session { get; set; }
    }
}
