using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Database.Models
{
    public class User
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }
}
