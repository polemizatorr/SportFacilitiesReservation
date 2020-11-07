using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportObjectsReservationSystem.Data;
using SportObjectsReservationSystem.Models;

namespace SportObjectsReservationSystem.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
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
            var reservation = await _context.Reservations
                .Include(m=>m.Date)
                .Include(m=>m.Date.Object)
                .Include(m=>m.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            var date = reservation.Date;
            date.IsReserved = false;
            
            var msg = new Message();

            var messages = _context.Messages.OrderByDescending(m => m.Id).FirstOrDefault();

            if (messages == null)
            {
                msg.Id = 1;
            }
            else
            {
                msg.Id = messages.Id + 1;
            }
            msg.IdTo = reservation.IdUser;
            msg.UserTo = reservation.User;
            msg.CreationDate = DateTime.Now;
            msg.Subject = "Reservation Deletion";
            msg.Content = "Your reservation on " + reservation.Date.StartDate + " at " + reservation.Date.Object.City +
                          " " + reservation.Date.Object.ObjectType +
                          " " + reservation.Date.Object.Street + " " + reservation.Date.Object.BuildingNumber +
                          " has been deleted by administrator.";
            _context.Messages.Add(msg);
            
            
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
 
        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AcceptReservation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation =  _context.Reservations.Find(id);
            reservation.Date =  _context.Dates.Find(reservation.IdDate);
            reservation.Date.IsReserved = true;
            reservation.User = _context.Users.Find(reservation.IdUser);
            reservation.Date.Object = _context.SportObjects.Find(reservation.Date.IdObject);
            
            var msg = new Message();

            var messages = _context.Messages.OrderByDescending(m => m.Id).FirstOrDefault();

            if (messages == null)
            {
                msg.Id = 1;
            }
            else
            {
                msg.Id = messages.Id + 1;
            }
            msg.IdTo = User.FindFirstValue(reservation.IdUser);
            msg.UserTo = _context.Users.Find(reservation.IdUser);
            msg.CreationDate = DateTime.Now;
            msg.Subject = "Reservation Confirmation";
            msg.Content = GenerateMsgAccept(reservation);

            await _context.Messages.AddAsync(msg);
            
            _context.Reservations.Update(reservation);

            var otherReservations = _context.Reservations.Where(x => x.Id != id && x.Date.Id == reservation.IdDate)
                .ToList(); // Returns other reservations related to the same Date

            var otherUsers = _context.Users.Where(x => x.Id != reservation.IdUser && reservation.IdUser == x.Id)
                .ToList(); // Returns users who also requested reservation for the same Date

            if (otherReservations.Any())
            {
                foreach (var item in otherReservations)
                {
                    _context.Reservations.Remove(item); // Deletes reservation requests from other users tha one who's been accepted 
                }
            }
            

            await _context.SaveChangesAsync();

            return RedirectToAction("ReservationActionSuccess", "Reservation");

        }
        
        public async Task<IActionResult> RejectReservation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation =  _context.Reservations.Find(id);
            reservation.Date =  _context.Dates.Find(reservation.IdDate);
            reservation.Date.IsReserved = true;
            reservation.User = _context.Users.Find(reservation.IdUser);
            reservation.Date.Object = _context.SportObjects.Find(reservation.Date.IdObject);
            
            var msg = new Message();
            var messages = _context.Messages.OrderByDescending(m => m.Id).FirstOrDefault();

            if (messages == null)
            {
                msg.Id = 1;
            }
            else
            {
                msg.Id = messages.Id + 1;
            }
            msg.IdTo = User.FindFirstValue(reservation.IdUser);
            msg.UserTo = _context.Users.Find(reservation.IdUser);
            msg.Subject = "Reservation Rejected";
            msg.CreationDate = DateTime.Now;
            msg.Content = GenerateMsgReject(reservation);
            await _context.Messages.AddAsync(msg);
            
            _context.Reservations.Remove(reservation);

            await _context.SaveChangesAsync();

            return RedirectToAction("ReservationActionSuccess", "Reservation");

        }

        public IActionResult ReservationActionSuccess()
        {
            return View();
        }

        public string GenerateMsgAccept(Reservation reservation)
        {
            var msgContent = "Your reservation for event starting on " + reservation.Date.StartDate + " on " + reservation.Date.Object.ObjectType + " placed in " + reservation.Date.Object.City + " " + reservation.Date.Object.Street + " " + reservation.Date.Object.BuildingNumber + " has ben Accepted.";
            return msgContent;
        }
        
        public string GenerateMsgReject(Reservation reservation)
        {
            var msgContent = "Your reservation for event starting on " + reservation.Date.StartDate + " on " + reservation.Date.Object.ObjectType + " placed in " + reservation.Date.Object.City + " " + reservation.Date.Object.Street + " " + reservation.Date.Object.BuildingNumber + " has ben Rejected.";
            return msgContent;
        }
        
    }
}