using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportObjectsReservationSystem.Data;
using SportObjectsReservationSystem.Models;

namespace SportObjectsReservationSystem.Services
{
    public class AdminUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly SportObjectsReservationContext _context;

        
        public AdminUserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            SportObjectsReservationContext context
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<User> CreateUser(string email, string password, string name, string surname, Boolean isAdmin)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                Name = name,
                Surname = surname,
                IsAdmin = isAdmin
                
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded) return null;
            
            if (user.IsAdmin)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return user;
        }
    }
}