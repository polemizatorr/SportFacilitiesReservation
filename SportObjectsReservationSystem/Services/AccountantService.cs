using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SportObjectsReservationSystem.Data;
using SportObjectsReservationSystem.Models;
using System.Linq;
using System.Threading.Tasks;



namespace SportObjectsReservationSystem.Services
{
    public class AccountantService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly SportObjectsReservationContext _context;
        private readonly IConfiguration _configuration;
        
        public AccountantService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            SportObjectsReservationContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }
        
        public async Task<User> Login(string email, string password)
        {
            var result = await _signInManager
                .PasswordSignInAsync(email, password, false, false);

            if (!result.Succeeded) return null;
            
            var user = _context.Users.Single(u => u.Email == email);

            return user;
        }
        
        public async Task<User> Register(string email, string password, string name, string surname)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                Name = name,
                Surname = surname
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded) return null;

            await _signInManager.SignInAsync(user, false);

            return user;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync(); 
        }
        
    }
}