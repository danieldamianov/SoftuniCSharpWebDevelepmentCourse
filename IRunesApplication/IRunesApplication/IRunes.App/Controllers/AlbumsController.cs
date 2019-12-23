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
    public class AlbumsController : BaseController
    {
        public IHttpResponse All(IHttpRequest httpRequest)
        {
            if (this.IsLogedIn(httpRequest) == false)
            {
                return this.Redirect("/Users/Login");
            }

            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {
                this.ViewData.Clear();
                this.ViewData.Add("@ListOfAlbums", string.Join("<br>", runesDbContext.Albums.Select(a => $"<a class=\"btn btn-primary\"  href=\"/Albums/Details?id={a.Id}\" >{a.Name}</a>")));
                return this.View();
            }
        }

        public IHttpResponse Create(IHttpRequest httpRequest)
        {
            return this.View();
        }

        public IHttpResponse HandleCreatingAlbum(IHttpRequest httpRequest)
        {
            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {
                string name = (string)httpRequest.FormData["name"];
                string cover = (string)httpRequest.FormData["cover"];

                runesDbContext.Albums.Add(new Album() { Name = HttpUtility.UrlDecode(name), Cover = HttpUtility.UrlDecode(cover), Id = Guid.NewGuid().ToString() });

                runesDbContext.SaveChanges();
            }

            return this.Redirect("/Albums/All");

        }

        public IHttpResponse Info(IHttpRequest httpRequest)
        {
            using (var dbContext = new IRunesDbContext())
            {
                Album album = dbContext.Albums.Find((string)httpRequest.QueryData["id"]);
                //string Html = File.ReadAllText("Controllers/DisplayInfoAboutAlbumPage.html");
                this.ViewData.Clear();
                this.ViewData.Add("@Link", album.Cover);
                this.ViewData.Add("@AlbumName", album.Name);
                this.ViewData.Add("@AlbumPrice", album.Price.ToString());
                this.ViewData.Add("@TracksLinks", string.Join("<br>", album.Tracks.Select(t => $"<a class=\"btn btn-primary\" href=\"/Tracks/Details?albumId={album.Id}&trackId={t.Id}\">{t.Name}</a>")));
                this.ViewData.Add("@albumId", album.Id);
                return this.View();
            }
        }

    }
}
