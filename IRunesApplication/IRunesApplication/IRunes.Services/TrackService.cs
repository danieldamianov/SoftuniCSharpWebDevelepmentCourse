using System;
using System.Collections.Generic;
using System.Text;
using IRunes.Database;
using IRunes.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace IRunes.Services
{
    public class TrackService : ITrackService
    {
        public TrackService()
        {
            this.dbContext = new IRunesDbContext();
        }

        private readonly IRunesDbContext dbContext;
        public Track GetTrackById(string id)
        {
            return this.dbContext.Tracks.Find(id);
        }
    }
}
