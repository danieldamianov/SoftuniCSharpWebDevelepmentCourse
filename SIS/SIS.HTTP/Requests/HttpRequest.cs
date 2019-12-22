using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Extensions;
using SIS.HTTP.Headers;
using SIS.HTTP.Sessions;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public IHttpSession Session { get; set; }

        private bool IsValidRequestLine(string[] requestLine)
        {
            if (requestLine.Length != 3 || requestLine[2] != GlobalConstants.HttpOneProtocolFragment)
            {
                return false;
            }

            return true;
        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
        {
            if (string.IsNullOrEmpty(queryString) || queryParameters.Length < 1)
            {
                return false;
            }

            return true;
        }

        private void ParseRequestMethod(string[] requestLine)
        {
            string requestMethodRaw = requestLine[0];

            this.RequestMethod = Enum.Parse<HttpRequestMethod>(requestMethodRaw.Capitalize());
        }
        private void ParseRequestUrl(string[] requestLine)
        {
            this.Url = requestLine[1];
        }

        private void ParseRequestPath()
        {
            this.Path = this.Url.Split('?', StringSplitOptions.None)[0];
        }

        private void ParseHeaders(string[] requestContent)
        {
            requestContent = requestContent.TakeWhile(rc => string.IsNullOrWhiteSpace(rc) == false).ToArray();
            foreach (var header in requestContent.Select(rc => rc.Split(": ")).Select(rcSplit => new HttpHeader(rcSplit[0], rcSplit[1])))
            {
                this.Headers.AddHeader(header);
            }
        }

        private void ParseCookies()
        {
            if (this.Headers.ContainsHeader("Cookie"))
            {
                foreach (var cookie in this.Headers.GetHeader("Cookie").Value
                    .Split("; ")
                    .Select(cookieAsString => cookieAsString.Split('='))
                    .Select(cookieAsArray => new HttpCookie(cookieAsArray[0],cookieAsArray[1])))
                {
                    this.Cookies.Add(cookie);
                }
            }
        }

        private void ParseQueryParameters()
        {
            if (this.Url.Contains('?') == false)
            {
                return;
            }
            string queryParamsString = this.Url.Substring(this.Url.IndexOf('?') + 1);
            string[] queryParams = queryParamsString.Split(new char[] { '&' });
            //queryParams = queryParams.Where(qp => qp.Contains('#') == false).ToArray();
            queryParams = queryParams.Select(queryParam => queryParam.Contains('#') == false ? queryParam : queryParam.Substring(0, queryParam.IndexOf('#'))).ToArray();
            this.IsValidRequestQueryString(queryParamsString, queryParams);
            foreach (var param in queryParams)
            {
                this.QueryData.Add(param.Split('=')[0], param.Split('=')[1]);
            }
        }

        private void ParseFormDataParameters(string formData)
        {
            if (string.IsNullOrEmpty(formData))
            {
                return;
            }
            string[] queryParams = formData.Split(new char[] { '&' });
            foreach (var param in queryParams)
            {
                this.FormData.Add(param.Split('=')[0], param.Split('=')[1]);
            }
        }

        private void ParseRequestParameters(string formData)
        {
            this.ParseQueryParameters();
            this.ParseFormDataParameters(formData);
        }
        private void ParseRequest(string formData)
        {
            string[] splitRequestContent = formData.Split(GlobalConstants.HttpNewLine, StringSplitOptions.None);

            string[] requestLine = splitRequestContent[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (this.IsValidRequestLine(requestLine) == false)
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLine);

            this.ParseRequestUrl(requestLine);

            this.ParseRequestPath();

            this.ParseHeaders(splitRequestContent.Skip(1).TakeWhile(rc => string.IsNullOrEmpty(rc) == false).ToArray());

            this.ParseCookies();

            if (string.IsNullOrEmpty(splitRequestContent[splitRequestContent.Length - 2]))
            {
                this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
            }
            else
	        {
                this.ParseRequestParameters(null);
            }
        }
    }
}
