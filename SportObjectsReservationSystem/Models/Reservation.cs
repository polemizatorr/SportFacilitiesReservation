using System;
using System.ComponentModel.DataAnnotations;

namespace SportObjectsReservationSystem.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public string IdUser { get; set; }
        
        public virtual User User { set; get; }
        public int IdDate { get; set; }

        public virtual Date Date { get; set; }
    }
}