using SIS.WebServer;
using SIS.WebServer.Routing;
using System;

namespace SIS.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Add(HTTP.Enums.HttpRequestMethod.Get, "/", request => new StartPageControler().LoadStartPage(request));
            serverRoutingTable.Add(HTTP.Enums.HttpRequestMethod.Get, "/login", request => new LoginControler().Login(request));
            serverRoutingTable.Add(HTTP.Enums.HttpRequestMethod.Get, "/register", request => new LoginControler().Register(request));
            serverRoutingTable.Add(HTTP.Enums.HttpRequestMethod.Get, "/home", request => new HomeController().Index(request));

            serverRoutingTable.Add(HTTP.Enums.HttpRequestMethod.Post, "/register", request => new LoginControler().RegisterHandle(request));
            serverRoutingTable.Add(HTTP.Enums.HttpRequestMethod.Post, "/login", request => new LoginControler().LoginHandle(request));

            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
