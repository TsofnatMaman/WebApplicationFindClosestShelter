using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Models;

namespace Dal
{
    public class ShelteredPlacesDb:DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Opinion> Opinsiones { get; set; }
        public DbSet<Shelter> Shelters { get; set; }

        public ShelteredPlacesDb(DbContextOptions<ShelteredPlacesDb> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().OwnsOne(a => a.Location);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        //    optionsBuilder.UseSqlServer("Server=LAPTOP_TSOFNAT\\SQLEXPRESS;Database=ShelteredPlacesDb;Trusted_Connection=True;TrustServerCertificate=True");
    }
}
