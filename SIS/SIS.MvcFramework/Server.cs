using SIS.HTTP.Sessions;
using SIS.MvcFramework.Routing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIS.MvcFramework
{
    public class Server
    {
        private const string LocalHostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener tcpListener;

        private readonly IServerRoutingTable serverRoutingTable;

        private readonly IHttpSessionStorage httpSessionStorage;

        private bool isRunning;

        public Server(int port, IServerRoutingTable serverRoutingTable, IHttpSessionStorage httpSessionStorage)
        {
            this.port = port;
            tcpListener = new TcpListener(IPAddress.Parse(LocalHostIpAddress), this.port);

            this.serverRoutingTable = serverRoutingTable;
            this.httpSessionStorage = httpSessionStorage;
        }

        public void Run()
        {
            tcpListener.Start();
            isRunning = true;

            Console.WriteLine($"Server started at http://{LocalHostIpAddress}:{port}");

            while (isRunning)
            {

                var client = tcpListener.AcceptSocketAsync().GetAwaiter().GetResult();

                Task.Run(() => Listen(client));
            }
        }

        public async Task Listen(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, serverRoutingTable, this.httpSessionStorage);
            //await Task.Run(() => Thread.Sleep(10000));
            await connectionHandler.ProcessRequestAsync();
        }
    }
}
