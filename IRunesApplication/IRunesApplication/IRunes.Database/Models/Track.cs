using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Database.Models
{
    public class Track
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }

        public string AlbumId { get; set; }

        public virtual Album Album { get; set; }
    }
}
