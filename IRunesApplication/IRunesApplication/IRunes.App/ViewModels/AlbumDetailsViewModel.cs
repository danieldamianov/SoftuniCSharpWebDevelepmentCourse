using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App.ViewModels
{
    public class AlbumDetailsViewModel
    {
        public string AlbumId { get; set; }

        public string AlbumName { get; set; }

        public string AlbumLink { get; set; }

        public decimal AlbumPrice { get; set; }

        public List<TrackViewModel> Tracks { get; set; }
    }
}
