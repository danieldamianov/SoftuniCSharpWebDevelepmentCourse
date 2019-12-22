using SIS.HTTP.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Cookies
{
    public class HttpCookie
    {
        private const string HttpCookieDefaultPath = "/";
        private const int HttpCookieDefaultExpirationDays = 3;

        public HttpCookie(string key, string value,int expires = HttpCookieDefaultExpirationDays, string path
             = HttpCookieDefaultPath)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            Key = key;
            Value = value;
            Expires = DateTime.UtcNow.AddDays(expires);
            Path = path;
            IsNew = true;
            HttpOnly = true;
        }

        public HttpCookie(string key, string value, bool isNew, int expires = HttpCookieDefaultExpirationDays, string path
             = HttpCookieDefaultPath) : this(key,value,expires,path)
        {
            IsNew = isNew;
        }

        public string Key { get; private set; }
        public string Value { get; private set; }
        public DateTime Expires { get;private set; }
        public string Path { get; set; }
        public bool IsNew { get; private set; }
        public bool HttpOnly { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"{this.Key}={this.Value}; Expires={this.Expires:R}");

            if (this.HttpOnly)
            {
                stringBuilder.Append("; HttpOnly");
            }

            stringBuilder.Append($"; Path={this.Path}");

            return stringBuilder.ToString();
        }

        public void Delete()
        {
            this.Expires = DateTime.UtcNow.AddDays(-1);
        }
    }
}
