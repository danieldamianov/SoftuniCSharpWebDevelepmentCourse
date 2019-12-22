using IRunes.Database;
using IRunes.Database.Models;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace IRunes.App.Controllers
{
    public class AlbumsController
    {
        public IHttpResponse LoadAllAblumsPage(IHttpRequest httpRequest)
        {
            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {
                if (runesDbContext.Sessions.Find(httpRequest.Session.Id) != null)
                {

                    Database.Models.Session session = runesDbContext.Sessions.Find(httpRequest.Session.Id);
                    User user = runesDbContext.Users.Find(session.UserId);
                    return new HtmlResult(File.ReadAllText("Controllers/AllAlbumsPage.html")
                        .Replace("@ListOfAlbums", string.Join("<br>", runesDbContext.Albums.Select(a => $"<a class=\"btn btn-primary btn-sm\" href=\"/Albums/Details?id={a.Id}\" >{a.Name}</a>")))
                    , SIS.HTTP.Enums.HttpResponseStatusCode.Ok); 

                }
                return new RedirectResult("/Users/Login");
            }
        }

        public IHttpResponse LoadCreateAlbumPage(IHttpRequest httpRequest)
        {
            //using (IRunesDbContext runesDbContext = new IRunesDbContext())
            //{

            //}

            return new HtmlResult(File.ReadAllText("Controllers/CreateAlbumPage.html"), SIS.HTTP.Enums.HttpResponseStatusCode.Ok); ;
        }

        internal IHttpResponse DisplayInfoAboutTrack(IHttpRequest request)
        {
            string albumId = (string)request.QueryData["albumId"];
            string trackId = (string)request.QueryData["trackId"];

            using (IRunesDbContext dbContext = new IRunesDbContext())
            {
                Album album = dbContext.Albums.Find(albumId);
                Track track = album.Tracks.First(t => t.Id == trackId);

                string path = "Controllers/DisplayInfoAboutTrack.html";

                return new HtmlResult(File.ReadAllText(path)
                    .Replace("@Link",track.Link)
                    .Replace("@Name",track.Name)
                    .Replace("@Price",track.Price.ToString())
                    .Replace("@albumId",album.Id),SIS.HTTP.Enums.HttpResponseStatusCode.Ok);
            }
        }

        internal IHttpResponse DisplayCreateTrackPage(IHttpRequest request)
        {
            string albumId = (string)request.QueryData["albumId"];
            return new HtmlResult(File.ReadAllText("Controllers/CreateTrackPage.html")
                .Replace("@albumId", albumId),SIS.HTTP.Enums.HttpResponseStatusCode.Ok);
        }

        public IHttpResponse HandleCreatingAlbum(IHttpRequest httpRequest)
        {
            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {
                string name = (string)httpRequest.FormData["name"];
                string cover = (string)httpRequest.FormData["cover"];

                runesDbContext.Albums.Add(new Album() { Name = name, Cover = HttpUtility.UrlDecode(cover), Id = Guid.NewGuid().ToString() });

                runesDbContext.SaveChanges();
            }

            return new RedirectResult("/Albums/All");

        }

        internal IHttpResponse HandleCreatingTrack(IHttpRequest request)
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
                    Name = trackName,
                    Link = HttpUtility.UrlDecode(link),
                    Price = price
                });

                runesDbContext.SaveChanges();
            }

            return new RedirectResult($"/Albums/Details?id={albumId}");
        }

        public IHttpResponse DisplayInfoAboutAlbum(IHttpRequest httpRequest)
        {
            using (var dbContext = new IRunesDbContext())
            {
                Album album = dbContext.Albums.Find((string)httpRequest.QueryData["id"]);
                string Html = File.ReadAllText("Controllers/DisplayInfoAboutAlbumPage.html");
                Html = Html.Replace("@Link", album.Cover);
                Html = Html.Replace("@AlbumName", album.Name);
                Html = Html.Replace("@AlbumPrice", album.Price.ToString());
                Html = Html.Replace("@TracksLinks", string.Join("<br>", album.Tracks.Select(t => $"<a href=\"/Tracks/Details?albumId={album.Id}&trackId={t.Id}\">{t.Name}</a>")));
                Html = Html.Replace("@albumId", album.Id);
                return new HtmlResult(Html, SIS.HTTP.Enums.HttpResponseStatusCode.Ok); ;
            }
        }

    }
}
