using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportObjectsReservationSystem.Data;
using SportObjectsReservationSystem.Models;

namespace SportObjectsReservationSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly SportObjectsReservationContext _context;

        public ReservationController(SportObjectsReservationContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reservations.Include(m=> m.Date).Include(m=>m.Date.Object).Include(m=>m.User).ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            var reservation = await _context.Reservations
                    .Include(m=>m.Date)
                    .Include(m=>m.Date.Object)
                    .Include(m=>m.User)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (reservation == null)
            {
                return NotFound();
            }
 
            return View(reservation);
        }
        
        public IActionResult Create()
        {
            return View();
        }
 
        // POST: SportObject/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUser,IdDate,Acceptance")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(reservation.IdUser);
                if (user == null)
                {
                    return NotFound();
                }
                reservation.User = user;
                
                var date = _context.Dates.Find(reservation.IdDate);
                
                if (date == null)
                {
                    return NotFound();
                }

                var sportObject = _context.SportObjects.Find(date.IdObject);
                if (sportObject == null)
                {
                    return NotFound();
                }

                date.Object = sportObject;
                
                reservation.Date = date;
                
                
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }
        
        // POST: SportObject/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUser,IdDate,Acceptance")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }
 
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _context.Users.Find(reservation.IdUser);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    reservation.User = user;
                
                    var date = _context.Dates.Find(reservation.IdDate);
                
                    if (date == null)
                    {
                        return NotFound();
                    }

                    var sportObject = _context.SportObjects.Find(date.IdObject);
                    if (sportObject == null)
                    {
                        return NotFound();
                    }

                    date.Object = sportObject;
                
                    reservation.Date = date;
                    
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }
        
        // GET: SportObject/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            var date = await _context.Reservations
                .Include(m=>m.Date)
                .Include(m=>m.Date.Object)
                .Include(m=>m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (date == null)
            {
                return NotFound();
            }
 
            return View(date);
        }
        
        // POST: SportObject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
 
        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
        
        
        
        
        
        
    }
}