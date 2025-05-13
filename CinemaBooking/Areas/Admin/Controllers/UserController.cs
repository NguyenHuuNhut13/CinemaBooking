using CinemaBooking.Models;
using CinemaBooking.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CinemaBooking.Data;

namespace CinemaBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly CinemaBookingDbContext _context;

        public UserController(CinemaBookingDbContext context)
        {
            _context = context;
        }

        // GET: Admin/User
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // GET: Admin/User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate email
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == viewModel.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View(viewModel);
                }

                // Ensure password is not empty and hash it before saving
                if (string.IsNullOrEmpty(viewModel.PasswordHash))
                {
                    ModelState.AddModelError("PasswordHash", "Password is required.");
                    return View(viewModel);
                }

                // Hashing password (use a proper hashing method, e.g., BCrypt or SHA256)
                var hashedPassword = viewModel.PasswordHash; // Replace with your hashing logic

                var user = new User
                {
                    FullName = viewModel.FullName,
                    Email = viewModel.Email,
                    PasswordHash = hashedPassword,
                    PhoneNumber = viewModel.PhoneNumber,
                    Role = viewModel.Role,
                    CreatedAt = DateTime.Now
                };

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }


        // GET: Admin/User/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            var viewModel = new UserViewModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };

            return View(viewModel);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel viewModel)
        {
            if (id != viewModel.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check for duplicate email in other users
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == viewModel.Email && u.UserId != id);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View(viewModel);
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FullName = viewModel.FullName;
                user.Email = viewModel.Email;
                user.PhoneNumber = viewModel.PhoneNumber;
                user.Role = viewModel.Role;

                // Update password only if a new one is provided
                if (!string.IsNullOrEmpty(viewModel.PasswordHash))
                {
                    // Hash the new password (use a proper hashing method)
                    user.PasswordHash =viewModel.PasswordHash; // Replace with your hashing logic
                }

                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        // Additional methods like validating unique email, etc. can be added here
    }
}
