using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportObjectsReservationSystem.Models;

namespace SportObjectsReservationSystem.Data
{
    public class SportObjectsReservationContext : IdentityDbContext<User, IdentityRole, string>
    {
        public SportObjectsReservationContext (DbContextOptions<SportObjectsReservationContext> options)
            : base(options)
        {
        }

        public DbSet<SportObject> SportObjects { get; set; }
       // public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "33bb94b2-52f4-40d4-8f7b-04834e83d9f2",
                    Name = "Dominik",
                    Surname = "Czerniak",
                    IsAdmin = true,
                    Email = "admin@vp.pl",
                    Password = "admin"

                });
        }
    }
}