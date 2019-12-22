using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace IRunes.Database.Models
{
    public class Album
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        [NotMapped]
        public decimal Price => this.Tracks.Sum(t => t.Price);

        public virtual ICollection<Track> Tracks { get; set; }
    }
}
