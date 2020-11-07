using System;
using System.ComponentModel.DataAnnotations;

namespace SportObjectsReservationSystem.Models
{
    public class Date
    {
        [Key]
        public int Id { get; set; }
        
        public int IdObject { get; set; }
        
        public virtual SportObject Object { set; get; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime EndDate { get; set; }
        
        public int MaxParticipants { get; set; }
        public bool IsReserved { get; set; }
    }
}