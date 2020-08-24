using System.ComponentModel.DataAnnotations;

namespace SportObjectsReservationSystem.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int IdFrom { get; set; }
        public int IdTo { get; set; }
        public string Content { get; set; }
    }
}