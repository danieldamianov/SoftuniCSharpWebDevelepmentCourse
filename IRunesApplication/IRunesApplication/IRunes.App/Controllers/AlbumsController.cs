using IRunes.App.ViewModels;
using IRunes.Database;
using IRunes.Database.Models;
using IRunes.Services;
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
        public AlbumsController()
        {
            this.albumsService = new AlbumsService();
        }

        IAlbumsService albumsService;

        [Authorize]
        public ActionResult All()
        {
            AlbumsAllViewModel albumCollectionViewModel = new AlbumsAllViewModel()
            {
                Albums = this.albumsService.GetAllAlbums().Select(album => new AlbumViewModel() { Id = album.Id , Name = album.Name}).ToList()
            };
            return this.View(albumCollectionViewModel);

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
            string name = (string)this.Request.FormData["name"];
            string cover = (string)this.Request.FormData["cover"];

            this.albumsService.AddAlbum(new Album() { Name = HttpUtility.UrlDecode(name), Cover = HttpUtility.UrlDecode(cover), Id = Guid.NewGuid().ToString() });

            return this.Redirect("/Albums/All");
        }

        [Authorize]
        public ActionResult Details()
        {
            Album album = this.albumsService.GetAlbumById((string)this.Request.QueryData["id"]);

            this.ViewData.Add("@TracksLinks", string.Join("<br>", album.Tracks.Select(t => $"<a class=\"btn btn-primary\" href=\"/Tracks/Details?albumId={album.Id}&trackId={t.Id}\">{t.Name}</a>")));
            return this.View(new AlbumDetailsViewModel() {
                AlbumId = album.Id,
                AlbumLink = album.Cover,
                AlbumName = album.Name,
                AlbumPrice = album.Price,
                Tracks = album.Tracks.Select(t => new TrackViewModel() { Id = t.Id, Name = t.Name}).ToList()});
        }

    }
}
