using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SportObjectsReservationSystem.Data;
using SportObjectsReservationSystem.Models;
using SportObjectsReservationSystem.Cryptology;
using SportObjectsReservationSystem.Services;

namespace SportObjectsReservationSystem.Controllers.AdminControllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly SportObjectsReservationContext _context;
        private readonly AdminUserService _service;

        public UserController(SportObjectsReservationContext context, AdminUserService service)
        {
            _context = context;
            _service = service;
        }


        // GET: User
        public IActionResult Index()
        {
            var users = from item in _context.Users select item;

            return View(users);
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Email,Password,IsAdmin")]
            User user)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateUser(user.Email, user.Password, user.Name, user.Surname, user.IsAdmin);
                
                
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

/*
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Surname,Email,Password,IsAdmin")]
            User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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

            return View(user);
        }
*/
        // GET: User/Delete/5
        public IActionResult AdminDeleteReject()
        {
            return View();
        }
        
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.IsAdmin)
            {
                return RedirectToAction("AdminDeleteReject");
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
 