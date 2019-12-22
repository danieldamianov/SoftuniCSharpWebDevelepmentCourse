using SIS.WebServer.Routing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIS.WebServer
{
    public class Server
    {
        private const string LocalHostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener tcpListener;

        private readonly IServerRoutingTable serverRoutingTable;

        private bool isRunning;

        public Server(int port,IServerRoutingTable serverRoutingTable)
        {
            this.port = port;
            this.tcpListener = new TcpListener(IPAddress.Parse(LocalHostIpAddress), this.port);

            this.serverRoutingTable = serverRoutingTable;
        }

        public void Run()
        {
            this.tcpListener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalHostIpAddress}:{this.port}");

            while (this.isRunning)
            {
                Console.WriteLine($"Waiting for a client...");

                var client = this.tcpListener.AcceptSocketAsync().GetAwaiter().GetResult();

                Task.Run(() => this.Listen(client));
            }
        }

        public async Task Listen(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable);
            //await Task.Run(() => Thread.Sleep(10000));
            await connectionHandler.ProcessRequestAsync();
        }
    }
}
