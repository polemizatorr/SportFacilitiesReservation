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
    public class DateController : Controller
    {
        private readonly SportObjectsReservationContext _context;

        public DateController(SportObjectsReservationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Dates.Include(m=> m.Object).ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
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
 
            return View(date);
        }
        
        // GET: Dates/Create
        public IActionResult Create()
        {
            return View();
        }
        
        // POST: Dates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdObject,StartDate,EndDate,MaxParticipants,IsReserved")] Date date)
        {
            if (ModelState.IsValid)
            {
                var sportObject = _context.SportObjects
                    .Find(date.IdObject);

                if (sportObject == null)
                {
                    return NotFound();
                }

                date.Object = sportObject;
                _context.Add(date);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(date);
        }
        
        // GET: SportObject/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            var date = await _context.Dates.FindAsync(id);
            if (date == null)
            {
                return NotFound();
            }
            return View(date);
        }
        
        // POST: SportObject/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdObject,Object,StartDate,EndDate,MaxParticipants,IsReserved")] Date date)
        {
            if (id != date.Id)
            {
                return NotFound();
            }
 
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(date);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DateExists(date.Id))
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
            return View(date);
        }
        
        // GET: SportObject/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            var date = await _context.Dates
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
            var date = await _context.Dates.FindAsync(id);
            _context.Dates.Remove(date);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
 
        private bool DateExists(int id)
        {
            return _context.Dates.Any(e => e.Id == id);
        }
    }
}