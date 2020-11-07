using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportObjectsReservationSystem.Data;

namespace SportObjectsReservationSystem.Controllers
{
    public class UserMessageController : Controller
    {
        private readonly SportObjectsReservationContext _context;

        public UserMessageController(SportObjectsReservationContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> UserMessages()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Find(userId);
            var messages = await _context.Messages.Include(m => m.UserTo).Where(d=>d.UserTo == user).ToListAsync();

            return View(messages);
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            var message = await _context.Messages.Include(m=>m.UserTo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }
 
            return View(message);
        }
    }
}