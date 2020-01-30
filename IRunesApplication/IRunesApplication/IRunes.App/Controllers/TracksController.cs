
using IRunes.App.ViewModels;
using IRunes.App.ViewModels.InputViewModels.Tracks;
using IRunes.Database.Models;
using IRunes.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.HttpAttributes;
using SIS.MvcFramework.Attributes.SecurityAttributes;
using SIS.MvcFramework.Results;

using System;
using System.Web;

namespace IRunes.App.Controllers
{
    public class TracksController : Controller
    {
        public TracksController(IAlbumsService albumsService, ITrackService trackService)
        {
            this.albumsService = albumsService;
            this.trackService = trackService;
        }

        private readonly IAlbumsService albumsService;
        private readonly ITrackService trackService;

        [Authorize]
        public ActionResult Create(string albumId)
        {
            return this.View(new TrackCreateViewModel { AlbumId = albumId});
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(TrackCreateInputViewModel model)
        {
            string albumId = model.AlbumId;

            string trackName = model.Name;

            string link = model.Link;

            decimal price = model.Price;

            this.albumsService.AddTrackToAlbum(albumId, new Track()
            {
                Id = Guid.NewGuid().ToString(),
                Name = HttpUtility.UrlDecode(trackName),
                Link = HttpUtility.UrlDecode(link),
                Price = price
            });

            return this.Redirect($"/Albums/Details?id={albumId}");
        }

        [Authorize]
        public ActionResult Details(TrackDetailsInputViewModel model)
        {
            string albumId = model.AlbumId;
            string trackId = model.TrackId;

            Track track = this.trackService.GetTrackById(trackId);

            return this.View(new TrackDetailsViewModel()
            {
                AlbumId = albumId,
                TrackLink = track.Link,
                TrackName = track.Name,
                TrackPrice = track.Price
            });
        }
    }
}
