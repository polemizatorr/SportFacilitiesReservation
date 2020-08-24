using System.ComponentModel.DataAnnotations;

namespace SportObjectsReservationSystem.Models
{
    public class SportObject
    {
        [Key]
        public int Id { get; set; }
        public string ObjectType { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; }
    }
}