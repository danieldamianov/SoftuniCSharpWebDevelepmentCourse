using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Cookies
{
    public interface IHttpCookieCollection 
    {
        bool ContainsCookie(string key);

        HttpCookie GetHttpCookie(string key);

        void Add(HttpCookie httpCookie);

        bool HasHttpCookies();
    }
}
