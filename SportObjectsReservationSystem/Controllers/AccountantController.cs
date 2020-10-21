using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportObjectsReservationSystem.Cryptology;
using SportObjectsReservationSystem.Data;
using SportObjectsReservationSystem.Models;
using SportObjectsReservationSystem.ModelsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SportObjectsReservationSystem.Services;


namespace SportObjectsReservationSystem.Controllers
{
    public class AccountantController : Controller
    {
        private readonly SportObjectsReservationContext _context;
        private readonly AccountantService _service;


        public AccountantController(SportObjectsReservationContext context, AccountantService service)
        {
            _context = context;
            _service = service;

        }
        
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind("Email,Password")] UserLoginDTO loginUser)
        {
            
            if (loginUser.Email == null || loginUser.Password == null)
            {
                return View("Login");
            }
            
            if (ModelState.IsValid)
            {
                var result = await _service.Login(loginUser.Email, loginUser.Password);
                if (result == null)
                {
                    ModelState.AddModelError(nameof(loginUser.Email),"Given data doesn't match any user.");
                    ModelState.AddModelError(nameof(loginUser.Password),"Given data doesn't match any user.");
                    return View();
                }

                HttpContext.Session.SetString("Email", loginUser.Email.ToString());
                
                
                return View("Index");
            }

            return View("Login");
        }
        
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Name,Surname,Email,Password,IsAdmin")] User user)
        {
            if (user.Name == null || user.Surname == null || user.Email == null || user.Password == null)
            {
                return View();
            }
            
            if (ModelState.IsValid)
            {
                var result = await _service.Register(user.Email, user.Password, user.Name, user.Surname);
                if (result == null)
                {
                    ModelState.AddModelError(nameof(user.Email),"Given data doesn't match any user.");
                    ModelState.AddModelError(nameof(user.Password),"Given data doesn't match any user.");
                    return View();
                }

                return RedirectToAction(nameof(Login));
            }
            
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return RedirectToAction("Login", "Accountant");
        }
    }
}