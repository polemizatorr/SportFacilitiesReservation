using System.ComponentModel.DataAnnotations;

namespace SportObjectsReservationSystem.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int IdFrom { get; set; }
        public virtual User UserFrom { set; get; }
        public int IdTo { get; set; }
        public virtual User UserTo { set; get; }
        public string Content { get; set; }
    }
}