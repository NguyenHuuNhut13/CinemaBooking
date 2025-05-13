using CinemaBooking.Areas.Admin.ViewModels;
using CinemaBooking.Data;
using CinemaBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace CinemaBooking.Areas.Admin.Controllers
{
    [Area("Admin")] // Specify the area
    public class MovieController : Controller
    {
        private readonly CinemaBookingDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // To handle file uploads

        public MovieController(CinemaBookingDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Movie
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }


        // GET: Movies/Details/5
        public IActionResult Details(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle poster upload
                var posterPath = await SavePosterFile(model.PosterFile);

                var movie = new Movie
                {
                    Title = model.Title,
                    Genre = model.Genre,
                    Director = model.Director,
                    Duration = model.Duration,
                    Rating = model.Rating,
                    ReleaseDate = model.ReleaseDate,
                    Description = model.Description,
                    TrailerUrl = model.TrailerUrl,
                    Poster = posterPath // Save the relative path
                };

                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // GET: Movies/Edit/5
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            // Map Movie to MovieViewModel
            var movieViewModel = new MovieViewModel
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Genre = movie.Genre,
                Director = movie.Director,
                Duration = movie.Duration,
                Rating = movie.Rating,
                ReleaseDate = movie.ReleaseDate,
                Poster = movie.Poster, // Assuming this is the file name or path, adjust as necessary
                TrailerUrl = movie.TrailerUrl,
                Description = movie.Description
            };

            return View(movieViewModel);
        }


        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieViewModel model)
        {
            if (id != model.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var movie = await _context.Movies.FindAsync(id);
                if (movie == null)
                {
                    return NotFound();
                }

                // Update movie details
                movie.Title = model.Title;
                movie.Genre = model.Genre;
                movie.Director = model.Director;
                movie.Duration = model.Duration;
                movie.Rating = model.Rating;
                movie.ReleaseDate = model.ReleaseDate;
                movie.Description = model.Description;
                movie.TrailerUrl = model.TrailerUrl;

                // Handle poster update
                if (model.PosterFile != null)
                {
                    // Delete old image file if it exists
                    if (!string.IsNullOrEmpty(movie.Poster))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, movie.Poster.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save new image file
                    movie.Poster = await SavePosterFile(model.PosterFile);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                // Delete the movie from the database
                _context.Movies.Remove(movie);

                // Delete the image file if it exists
                if (!string.IsNullOrEmpty(movie.Poster))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, movie.Poster.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }



        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }

        // Helper method to handle file uploads
        private async Task<string> SavePosterFile(IFormFile? posterFile)
        {
            if (posterFile == null || posterFile.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + posterFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await posterFile.CopyToAsync(fileStream);
            }

            return uniqueFileName; // Return the relative path
        }
    }
}
