using Microsoft.EntityFrameworkCore;
using SIS.Demo.Models;
using System;

namespace SIS.Data
{
    public class TestDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-B3I8JPR\\SQLEXPRESS;Database=Test111;Integrated Security = true");
            base.OnConfiguring(optionsBuilder);
        }
    }
}