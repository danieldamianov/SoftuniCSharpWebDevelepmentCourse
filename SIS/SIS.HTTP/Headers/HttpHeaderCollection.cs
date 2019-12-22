using SIS.HTTP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.HTTP.Headers
{
    class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void AddHeader(HttpHeader httpHeader)
        {
            this.headers.Add(httpHeader.Key, httpHeader);
        }

        public bool ContainsHeader(string key)
        {
            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (this.headers.ContainsKey(key))
            {
                return this.headers[key];
            }
            return null;
        }

        public override string ToString()
        {
            return string.Join(GlobalConstants.HttpNewLine, this.headers.Values.Select(v => v.ToString()));
        }
    }
}
