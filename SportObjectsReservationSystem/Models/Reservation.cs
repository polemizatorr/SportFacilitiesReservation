using System.ComponentModel.DataAnnotations;

namespace SportObjectsReservationSystem.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdDate { get; set; }
        public bool Acceptance { get; set; }
    }
}