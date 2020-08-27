using System.ComponentModel.DataAnnotations;

namespace SportObjectsReservationSystem.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public int IdUser { get; set; }
        
        public virtual User User { set; get; }
        public int IdDate { get; set; }
        
        public virtual Date Date { get; set; }
        public bool Acceptance { get; set; }
    }
}