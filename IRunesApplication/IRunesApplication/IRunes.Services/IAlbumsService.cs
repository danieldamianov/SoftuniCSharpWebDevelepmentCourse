using IRunes.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Services
{
    public interface IAlbumsService
    {
        ICollection<Album> GetAllAlbums();

        Album AddAlbum(Album album);

        Album GetAlbumById(string id);

        bool AddTrackToAlbum(string albumId, Track track);
    }
}
