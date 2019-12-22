using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.HTTP.Responses
{
    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Content = new byte[0];
            this.Cookies = new HttpCookieCollection();
        }

        public HttpResponse(HttpResponseStatusCode httpResponseStatusCode) : this()
        {
            CoreValidator.ThrowIfNull(httpResponseStatusCode,nameof(httpResponseStatusCode));
            this.StatusCode = httpResponseStatusCode;
        }
        
        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; }

        public byte[] Content { get; set; }

        public IHttpCookieCollection Cookies { get; }

        public void AddHeader(HttpHeader httpHeader)
        {
            CoreValidator.ThrowIfNull(httpHeader, nameof(httpHeader));
            this.Headers.AddHeader(httpHeader);
        }

        public void AddCookie(HttpCookie httpCookie)
        {
            CoreValidator.ThrowIfNull(httpCookie, nameof(httpCookie));
            this.Cookies.Add(httpCookie);
        }

        public byte[] GetBytes()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(this.ToString());
            bytes = bytes.Concat(this.Content).ToArray();
            return bytes;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"{GlobalConstants.HttpOneProtocolFragment} {(int)this.StatusCode} {this.StatusCode}");
            stringBuilder.Append(GlobalConstants.HttpNewLine);
            stringBuilder.Append(this.Headers);
            stringBuilder.Append(GlobalConstants.HttpNewLine);

            if (this.Cookies.HasHttpCookies())
            {
                stringBuilder.Append($"Set-Cookie: {this.Cookies}").Append(GlobalConstants.HttpNewLine);
            }

            stringBuilder.Append(GlobalConstants.HttpNewLine);

            return stringBuilder.ToString();
        }
    }
}
