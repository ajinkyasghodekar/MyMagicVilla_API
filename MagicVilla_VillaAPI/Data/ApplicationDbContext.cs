using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        // DbSet for adding Security in our application
        public DbSet<LocalUser> LocalUsers { get; set; }

        // DbSet for villa details (Villas is a table name we want in our db)
        public DbSet<Villa> Villas { get; set; }

        // DbSet for villa number (VillaNumbers is a table name we want in our db)
        public DbSet<VillaNumber> VillaNumbers { get; set; }

        // Inserting sample data to table
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(

                new Villa()
                {

                    Id = 1,
                    Name = "Royal Villa",
                    Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitus fringilla enim.",
                    ImageUrl = "https://www.pexels.com/search/villa/",
                    Occupancy = 5,
                    Rate = 220,
                    Sqft = 550,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
                new Villa
                {

                    Id = 2,
                    Name = "Premium Pool  Villa",
                    Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitus fringilla enim.",
                    ImageUrl = "https://www.pexels.com/search/villa/",
                    Occupancy = 5,
                    Rate = 320,
                    Sqft = 550,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
                new Villa
                {

                    Id = 3,
                    Name = "Luxury Pool  Villa",
                    Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitus fringilla enim.",
                    ImageUrl = "https://www.pexels.com/search/villa/",
                    Occupancy = 5,
                    Rate = 400,
                    Sqft = 750,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                },
                new Villa
                {

                    Id = 4,
                    Name = "Dimond  Villa",
                    Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitus fringilla enim.",
                    ImageUrl = "https://www.pexels.com/search/villa/",
                    Occupancy = 5,
                    Rate = 550,
                    Sqft = 900,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                }, 
                new Villa
                {

                    Id = 5,
                    Name = "Diamond  Pool  Villa",
                    Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitus fringilla enim.",
                    ImageUrl = "https://www.pexels.com/search/villa/",
                    Occupancy = 5,
                    Rate = 600,
                    Sqft = 1100,
                    Amenity = "",
                    CreatedDate = DateTime.Now
                });
        }
    }

}
