﻿using IRunes.Database;
using IRunes.Database.Models;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace IRunes.App.Controllers
{
    class TracksController : BaseController
    {
        public IHttpResponse Create(IHttpRequest request)
        {
            string albumId = (string)request.QueryData["albumId"];
            this.ViewData.Clear();
            this.ViewData.Add("@albumId", albumId);
            return this.View();
        }
        public IHttpResponse HandleCreatingTrack(IHttpRequest request)
        {
            string albumId = (string)request.QueryData["albumId"];

            string trackName = (string)request.FormData["name"];
            string link = (string)request.FormData["link"];
            decimal price = decimal.Parse((string)request.FormData["price"]);

            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {
                runesDbContext.Albums.Find(albumId).Tracks.Add(new Track()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = HttpUtility.UrlDecode(trackName),
                    Link = HttpUtility.UrlDecode(link),
                    Price = price
                });

                runesDbContext.SaveChanges();
            }

            return this.Redirect($"/Albums/Details?id={albumId}");
        }
        public IHttpResponse Info(IHttpRequest request)
        {
            string albumId = (string)request.QueryData["albumId"];
            string trackId = (string)request.QueryData["trackId"];

            using (IRunesDbContext dbContext = new IRunesDbContext())
            {
                Album album = dbContext.Albums.Find(albumId);
                Track track = album.Tracks.First(t => t.Id == trackId);
                this.ViewData.Clear();
                this.ViewData.Add("@Link", track.Link);
                this.ViewData.Add("@Name", track.Name);
                this.ViewData.Add("@Price", track.Price.ToString());
                this.ViewData.Add("@albumId", album.Id);

                return this.View();

            }
        }
    }
}
