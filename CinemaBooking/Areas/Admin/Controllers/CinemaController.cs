using CinemaBooking.Areas.Admin.ViewModels;
using CinemaBooking.Data;
using CinemaBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Add this for async operations

namespace CinemaBooking.Areas.Admin.Controllers
{
    [Area("Admin")] // Important: Specify the area
    public class CinemaController : Controller
    {
        private readonly CinemaBookingDbContext _context; // Your DbContext

        public CinemaController(CinemaBookingDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Cinema
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var totalRecords = await _context.Cinemas.CountAsync();
            var cinemas = await _context.Cinemas
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            var viewModel = new PaginatedCinemaViewModel
            {
                Cinemas = cinemas,
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };

            return View(viewModel);
        }


        // GET: Admin/Cinema/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Cinema/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CinemaViewModel cinemaViewModel)
        {
            if (ModelState.IsValid)
            {
                // Map ViewModel to Domain Model
                var cinema = new Cinema
                {
                    Name = cinemaViewModel.Name,
                    Location = cinemaViewModel.Location,
                    PhoneNumber = cinemaViewModel.PhoneNumber,
                    City = cinemaViewModel.City
                };

                _context.Add(cinema);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cinemaViewModel);
        }

        // GET: Admin/Cinema/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return NotFound();
            }

            // Map Domain Model to ViewModel
            var cinemaViewModel = new CinemaViewModel
            {
                Name = cinema.Name,
                Location = cinema.Location,
                PhoneNumber = cinema.PhoneNumber,
                City = cinema.City
            };

            return View(cinemaViewModel);
        }

        // POST: Admin/Cinema/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CinemaViewModel cinemaViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing cinema from the database using id 
                    var cinema = await _context.Cinemas.FindAsync(id);

                    if (cinema == null)
                    {
                        return NotFound();
                    }

                    // Update the cinema entity with values from the view model
                    cinema.Name = cinemaViewModel.Name;
                    cinema.Location = cinemaViewModel.Location;
                    cinema.PhoneNumber = cinemaViewModel.PhoneNumber;
                    cinema.City = cinemaViewModel.City;

                    _context.Update(cinema);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency exceptions (if needed)
                    // ... 
                    throw;
                }
            }

            // If ModelState is not valid or there are errors, return to the view
            return View(cinemaViewModel);
        }


        // POST: Admin/Cinema/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema != null)
            {
                _context.Cinemas.Remove(cinema);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CinemaExists(int id)
        {
            return _context.Cinemas.Any(e => e.CinemaId == id);
        }
    }
}