using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.Routing
{
    public interface IServerRoutingTable
    {
        void Add(HttpRequestMethod httpRequestMethod, string path, Func<IHttpRequest, IHttpResponse> func);

        bool Contains(HttpRequestMethod httpRequestMethod, string path);

        Func<IHttpRequest, IHttpResponse> Get(HttpRequestMethod httpRequestMethod, string path);
    }
}
