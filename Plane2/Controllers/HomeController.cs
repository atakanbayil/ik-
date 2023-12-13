using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Plane2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Plane2.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Ensure the user is not already logged in
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check hardcoded credentials
            if (model.Email == "admin@example.com" && model.Password == "Password123!")
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Email),
            // Add more claims if needed
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // For 'remember me' functionality
                };

                await HttpContext.SignInAsync("YourCookieAuthScheme", principal, authProperties);


                // Redirect to the root of the application
                return LocalRedirect("/");
            }

            // If login fails, redisplay form
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protect against CSRF attacks
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle registration logic here, such as:
                // 1. Check if the user already exists
                // 2. Hash the password
                // 3. Save the user to the database

                // After successful registration, redirect to the login page or other destination
                return RedirectToAction("Login");
            }

            // If we got this far, something failed, redisplay the form
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            // Log the logout attempt
            _logger.LogInformation("User logged out at {Time}.", DateTime.UtcNow);

            // Sign out the user
            await HttpContext.SignOutAsync("YourCookieAuthScheme");

            // Optional: Perform any additional cleanup here

            // Redirect to the home page with a logout confirmation message
            TempData["LogoutMessage"] = "You have been successfully logged out.";
            return LocalRedirect("/");
        }


        [HttpGet]
        public IActionResult SearchList(string fromDestination, string toDestination, DateTime? returnDate)
        {
            // Implement your search logic here
            // For example, you might query a database using the parameters

            // Then return the searchlist view
            return View("Searchlist");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
