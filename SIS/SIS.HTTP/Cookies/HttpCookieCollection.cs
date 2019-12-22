using SIS.HTTP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.HTTP.Cookies
{
    public class HttpCookieCollection : IHttpCookieCollection
    {
        private Dictionary<string,HttpCookie> httpCookies;

        public HttpCookieCollection()
        {
            this.httpCookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie httpCookie)
        {
            CoreValidator.ThrowIfNull(httpCookie, nameof(httpCookie));

            this.httpCookies[httpCookie.Key] = httpCookie;
        }

        public bool ContainsCookie(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            return this.httpCookies.ContainsKey(key);
        }

        public HttpCookie GetHttpCookie(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            if (this.ContainsCookie(key) == false)
            {
                throw new ArgumentException("Doesnt exist cookie with given name");
            }

            return this.httpCookies[key];
        }

        public bool HasHttpCookies()
        {
            return this.httpCookies.Count != 0;
        }

        public const string HttpCookeStringSeparator = GlobalConstants.HttpNewLine + "Set-Cookie: ";

        public override string ToString()
        {
            return string.Join(HttpCookeStringSeparator,this.httpCookies.Values);
        }
    }
}
