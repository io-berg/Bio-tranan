using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTheaterCore.Models;

namespace Infrastructure.Data
{
    public class MovieTheaterContext : DbContext
    {
        public MovieTheaterContext(DbContextOptions<MovieTheaterContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Salon> Salons { get; set; }
        public DbSet<MovieViewing> Viewings { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
    }
}