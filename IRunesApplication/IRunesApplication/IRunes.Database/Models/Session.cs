using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Database.Models
{
    public class Session
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
