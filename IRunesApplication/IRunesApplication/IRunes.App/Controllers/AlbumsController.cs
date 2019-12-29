using IRunes.Database;
using IRunes.Database.Models;

using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.HttpAttributes;
using SIS.MvcFramework.Attributes.SecurityAttributes;
using SIS.MvcFramework.Results;

using System;
using System.Linq;
using System.Web;

namespace IRunes.App.Controllers
{
    public class AlbumsController : Controller
    {
        [Authorize]
        public ActionResult All()
        {
            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {
                this.ViewData.Clear();
                this.ViewData.Add("@ListOfAlbums", string.Join("<br>", runesDbContext.Albums.Select(a => $"<a class=\"btn btn-primary\"  href=\"/Albums/Details?id={a.Id}\" >{a.Name}</a>")));
                return this.View();
            }
        }

        [Authorize]
        public ActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost(ActionName = "Create")]
        public ActionResult HandleCreatingAlbum()
        {
            using (IRunesDbContext runesDbContext = new IRunesDbContext())
            {
                string name = (string)this.Request.FormData["name"];
                string cover = (string)this.Request.FormData["cover"];

                runesDbContext.Albums.Add(new Album() { Name = HttpUtility.UrlDecode(name), Cover = HttpUtility.UrlDecode(cover), Id = Guid.NewGuid().ToString() });

                runesDbContext.SaveChanges();
            }

            return this.Redirect("/Albums/All");
        }

        [Authorize]
        public ActionResult Details()
        {
            using (var dbContext = new IRunesDbContext())
            {
                Album album = dbContext.Albums.Find((string)this.Request.QueryData["id"]);
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
