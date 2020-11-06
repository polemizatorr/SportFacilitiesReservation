using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SportObjectsReservationSystem.Models
{
    public class User : IdentityUser
    {
        //[Key]
        //public int Id { get; set; }
       // public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsAdmin { get; set; }
    }
}