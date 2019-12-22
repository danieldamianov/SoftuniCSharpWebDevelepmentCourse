using IRunes.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace IRunes.Database
{
    public class IRunesDbContext : DbContext
    {
        //public IRunesDbContext()
        //{
        //    Users = new ;
        //    Tracks = tracks;
        //    Albums = albums;
        //    Sessions = sessions;
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public bool Single { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-B3I8JPR\\SQLEXPRESS;Database=IRunesDb;Integrated Security = true");
            optionsBuilder.UseLazyLoadingProxies(true);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Track>()
                .HasOne(t => t.Album)
                .WithMany(a => a.Tracks)
                .HasForeignKey(t => t.AlbumId);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UserId);
        }
    }
}
