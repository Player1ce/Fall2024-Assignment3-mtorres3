using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_mtorres3.Data;
using Fall2024_Assignment3_mtorres3.Models;

namespace Fall2024_Assignment3_mtorres3.Controllers
{
    public class MovieReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MovieReviews
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MovieReview.Include(m => m.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MovieReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieReview = await _context.MovieReview
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movieReview == null)
            {
                return NotFound();
            }

            return View(movieReview);
        }

        // GET: MovieReviews/Create
        public IActionResult Create()
        {
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "ID");
            return View();
        }

        // POST: MovieReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieID,ID,Text,Positive,Negative,Neutral,Compound")] MovieReview movieReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "ID", movieReview.MovieID);
            return View(movieReview);
        }

        // GET: MovieReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieReview = await _context.MovieReview.FindAsync(id);
            if (movieReview == null)
            {
                return NotFound();
            }
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "ID", movieReview.MovieID);
            return View(movieReview);
        }

        // POST: MovieReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieID,ID,Text,Positive,Negative,Neutral,Compound")] MovieReview movieReview)
        {
            if (id != movieReview.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieReviewExists(movieReview.ID))
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
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "ID", movieReview.MovieID);
            return View(movieReview);
        }

        // GET: MovieReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieReview = await _context.MovieReview
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movieReview == null)
            {
                return NotFound();
            }

            return View(movieReview);
        }

        // POST: MovieReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieReview = await _context.MovieReview.FindAsync(id);
            if (movieReview != null)
            {
                _context.MovieReview.Remove(movieReview);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieReviewExists(int id)
        {
            return _context.MovieReview.Any(e => e.ID == id);
        }
    }
}
