using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Routing;
using SIS.HTTP.Common;
using SIS.HTTP.Exceptions;
using SIS.WebServer.Results;
using System.Threading.Tasks;
using SIS.HTTP.Sessions;
using System.Reflection;
using System.IO;

namespace SIS.WebServer
{
    class ConnectionHandler
    {
        private readonly Socket client;
        private readonly IServerRoutingTable serverRoutingTable;

        public ConnectionHandler(Socket client, IServerRoutingTable serverRoutingTable)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(serverRoutingTable, nameof(serverRoutingTable));

            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
        }

        public async Task ProcessRequestAsync()
        {
            try
            {
                var request = await this.ReadRequestAsync();

                if (request != null)
                {
                    Console.WriteLine($"Processing: {request.RequestMethod} {request.Path}...");
                    var sessionId = this.SetRequestSession(request);
                    var response = this.HandleRequest(request);
                    this.SetResponseSession(response, sessionId);
                    //response.AddCookie(new HTTP.Cookies.HttpCookie("testDelimiterCookie", "value"));
                    await this.PrepareResponseAsync(response);
                }
            }
            catch (BadRequestException ex)
            {
                await this.PrepareResponseAsync(new TextResult(ex.ToString(), HTTP.Enums.HttpResponseStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                await this.PrepareResponseAsync(new TextResult(ex.ToString(), HTTP.Enums.HttpResponseStatusCode.InternalServerError));
            }
            this.client.Shutdown(SocketShutdown.Both);
        }

        private void SetResponseSession(IHttpResponse httpResponse, string sessionId)
        {
            if (sessionId != null)
            {
                httpResponse.AddCookie(new HTTP.Cookies.HttpCookie(HttpSessionStorage.SessionCookieKey, sessionId));
            }
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId = null;

            if (httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
            {
                var cookie = httpRequest.Cookies.GetHttpCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }

            return sessionId;
        }

        private async Task<IHttpRequest> ReadRequestAsync()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                var bytesAsStirng = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);

                result.Append(bytesAsStirng);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }
            return new HttpRequest(result.ToString());
        }

        private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        
        {
            //if (this.serverRoutingTable.Contains(httpRequest.RequestMethod, httpRequest.Path) == false)
            //{
            //    return new TextResult($"Route with method {httpRequest.RequestMethod} and path \"{httpRequest.Path}\" not found."
            //        , HTTP.Enums.HttpResponseStatusCode.NotFound);
            //}
            if (this.serverRoutingTable.Contains(httpRequest.RequestMethod, httpRequest.Path) == false)
            {
                return this.ReturnIfResource(httpRequest.Path);
            }


            var response = this.serverRoutingTable.Get(httpRequest.RequestMethod, httpRequest.Path).Invoke(httpRequest);
            return response;
        }

        private IHttpResponse ReturnIfResource(string path)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            string pathToAssembly = assembly.Location;
            string fullPath = $@"{pathToAssembly}/../../../../Resources{path}";
            if (File.Exists(fullPath))
            {
                byte[] content = File.ReadAllBytes(fullPath);
                return new InlineResourceResult(content, HTTP.Enums.HttpResponseStatusCode.Ok);
            }
            else
            {
                return new TextResult("Not Found :" + path, HTTP.Enums.HttpResponseStatusCode.NotFound);
            }
        }

        private async Task PrepareResponseAsync(IHttpResponse httpResponse)
        {
            byte[] bytes = httpResponse.GetBytes();
            await this.client.SendAsync(bytes, SocketFlags.None);
        }
    }
}

