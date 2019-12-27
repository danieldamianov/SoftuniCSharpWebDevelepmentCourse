using IRunes.App.Controllers;
using IRunes.Database;
using SIS.HTTP.Enums;
using SIS.MvcFramework;
using SIS.MvcFramework.Results;
using SIS.MvcFramework.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App
{
    public class StartUp : IMvcApplication
    {
        public void Configure(IServerRoutingTable serverRoutingTable)
        {
            using(IRunesDbContext dbContext = new IRunesDbContext())
            {
                dbContext.Database.EnsureCreated();
            }

            //serverRoutingTable.Add(HttpRequestMethod.Get, "/", (_) => new RedirectResult("/Home/Index"));
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Home/Index", new HomeController().HomePage);

            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Users/Login", new UsersController().Login);
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Users/Register", new UsersController().Register);
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Albums/All", new AlbumsController().All);
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Albums/Create", new AlbumsController().Create);
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Albums/Details", new AlbumsController().Info);
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Tracks/Create", new TracksController().Create);
            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Tracks/Details", new TracksController().Info);


            //serverRoutingTable.Add(HttpRequestMethod.Get, "/Users/Logout", new UsersController().HandleLoggingOut);
            //serverRoutingTable.Add(HttpRequestMethod.Post, "/Users/Login", new UsersController().HandleLogingIn);
            //serverRoutingTable.Add(HttpRequestMethod.Post, "/Users/Register", new UsersController().HandleRegistration);
            //serverRoutingTable.Add(HttpRequestMethod.Post, "/Albums/Create", new AlbumsController().HandleCreatingAlbum);
            //serverRoutingTable.Add(HttpRequestMethod.Post, "/Tracks/Create", new TracksController().HandleCreatingTrack);
        }

        public void ConfigureServices()
        {

        }
    }
}
