using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportObjectsReservationSystem.Data;
using SportObjectsReservationSystem.Models;

namespace SportObjectsReservationSystem.Controllers
{
    public class UserReservationController : Controller
    {
        private readonly SportObjectsReservationContext _context;

        public UserReservationController(SportObjectsReservationContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dates.Include(m=> m.Object).ToListAsync());
        }
        
        public async Task<IActionResult> UserReservations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservations = await _context.Reservations.Include(m => m.Date).Include(m => m.Date.Object)
                .Include(m => m.User).ToListAsync();

            var userReservations = from item in reservations where item.IdUser == userId select item;
            
            
            return View(userReservations);
        }

        public async Task<IActionResult> ReserveDate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var date = await _context.Dates.Include(m=>m.Object)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (date == null)
            {
                return NotFound();
            }


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userReservations = from item in _context.Reservations where item.IdUser == userId select item;

            if (userReservations.Count() != 0)
            {
                return View("ReservationWarning");
            }

            var idReservation = 0;
            var LastReservation = _context.Reservations.OrderByDescending(x => x.Id).FirstOrDefault();
            
            if (LastReservation == null)
            {
                idReservation = 1;
            }
            else
            {
                idReservation = LastReservation.Id + 1;
            }


            var NewReservation = new Reservation();
            
            NewReservation.Id = idReservation;
            NewReservation.IdUser = user.Id;
            NewReservation.User = user;
            NewReservation.IdDate = date.Id;
            NewReservation.Date = date;

            await _context.Reservations.AddAsync(NewReservation);
            await _context.SaveChangesAsync();
            return View("ReserveDate");
        }
        
    }
}