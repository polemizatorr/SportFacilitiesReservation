using Microsoft.EntityFrameworkCore;
using SportObjectsReservationSystem.Models;

namespace SportObjectsReservationSystem.Data
{
    public class SportObjectsReservationContext : DbContext
    {
        public SportObjectsReservationContext (DbContextOptions<SportObjectsReservationContext> options)
            : base(options)
        {
        }

        public DbSet<SportObject> SportObjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}