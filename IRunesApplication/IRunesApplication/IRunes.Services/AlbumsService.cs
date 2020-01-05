using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRunes.Database;
using IRunes.Database.Models;

namespace IRunes.Services
{
    public class AlbumsService : IAlbumsService
    {
        private readonly IRunesDbContext runesDbContext;

        public AlbumsService()
        {
            this.runesDbContext = new IRunesDbContext();
        }

        public Album AddAlbum(Album album)
        {
            album = this.runesDbContext.Albums.Add(album).Entity;
            this.runesDbContext.SaveChanges();
            return album;
        }

        public bool AddTrackToAlbum(string albumId, Track track)
        {
            Album album = this.GetAlbumById(albumId);

            if (track == null || string.IsNullOrWhiteSpace(albumId) || album == null)
            {
                return false;
            }

            album.Tracks.Add(track);

            this.runesDbContext.SaveChanges();

            return true;
        }

        public Album GetAlbumById(string id)
        {
            return this.runesDbContext.Albums.Find(id);
        }

        public ICollection<Album> GetAllAlbums()
        {
            return this.runesDbContext.Albums.ToList();
        }
    }
}
