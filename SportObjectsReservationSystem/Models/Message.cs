using System;
using System.ComponentModel.DataAnnotations;

namespace SportObjectsReservationSystem.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string IdTo { get; set; }
        public virtual User UserTo { set; get; }
        public string Subject { set; get; }
        public string Content { get; set; }
        
        public DateTime CreationDate { get; set; }
    }
}