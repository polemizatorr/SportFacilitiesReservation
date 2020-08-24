using System;
using System.ComponentModel.DataAnnotations;

namespace SportObjectsReservationSystem.Models
{
    public class Date
    {
        [Key]
        public int Id { get; set; }
        
        public int IdObject { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        
        public int NoParticipants { get; set; }
        public bool IsReserved { get; set; }
    }
}