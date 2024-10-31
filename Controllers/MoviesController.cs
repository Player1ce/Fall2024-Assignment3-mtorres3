using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_mtorres3.Data;
using Fall2024_Assignment3_mtorres3.Models;
using System.Numerics;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using Azure;
using System.Text.RegularExpressions;
using VaderSharp2;


namespace Fall2024_Assignment3_mtorres3.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ChatClient _chatClient;
        private readonly SentimentIntensityAnalyzer _analyzer;

        public MoviesController(ApplicationDbContext context, ChatClient chatClient)
        {
            _context = context;
            _chatClient = chatClient;
            _analyzer = new SentimentIntensityAnalyzer();
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movie.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                //.Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            //var reviews = await _context.MovieReview
            //    .Include(mr => mr.)

            var actors = await _context.MovieActor
                .Include(ma => ma.Actor)
                .Where(ma => ma.MovieID == movie.ID)
                .Select(ma => ma.Actor)
                .ToListAsync();

            var vm = new MovieDetailsViewModel(movie, actors);

            return View(vm);
        }

        // GET: Movies/Create
         public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Genre,ReleaseYear,ImdbHyperlink")] Movie movie, IFormFile? poster)
        {
            if (ModelState.IsValid)
            {
                if (poster != null && poster.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await poster.CopyToAsync(memoryStream);
                    movie.Poster= memoryStream.ToArray();
                }

                try
                {

                    ChatCompletion completion = await _chatClient.CompleteChatAsync(
                    [
                        // System messages represent instructions or other guidance about how the assistant should behave
                        new SystemChatMessage(
                            "You are a group of ten movie reviewers with each with a unique, refined, and accurate sense for movie quality and characteristics. Although you can share ideas, you each give individual reviews."),
                        // User messages represent user input, whether historical or the most recent input
                        new UserChatMessage("Please generate ten movie reviews about " + movie.Title +
                                            ". Each review should be by an individual reviewer. Please organize the reviews into an ordered list numbered 1 through 10, separated by newlines, and with format 1. <text>"),
                    ]);

                    Console.WriteLine($"{completion.Role} : \n{completion.Content[0].Text}");
                    string input = completion.Content[0].Text;

                    // Split the input string into individual items based on newline
                    string[] items = input.Split('\n');

                    // Regex pattern to match the structure "1. <text>"
                    string pattern = @"^\d+\.\s*(.*)$";

                    // List to store extracted texts
                    List<string> extractedTexts = new List<string>();

                    foreach (var item in items)
                    {
                        Match match = Regex.Match(item, pattern);
                        if (match.Success)
                        {
                            // Add the extracted text (group 1) to the list
                            extractedTexts.Add(match.Groups[1].Value);
                        }
                    }

                    foreach (string text in extractedTexts)
                    {
                        Console.WriteLine("");
                        Console.WriteLine($"review: {text}");
                        Console.WriteLine("");
                        var results = _analyzer.PolarityScores(text);
                        Console.WriteLine($"Positive: {results.Positive}");
                        Console.WriteLine($"Negative: {results.Negative}");
                        Console.WriteLine($"Neutral: {results.Neutral}");
                        Console.WriteLine($"Compound: {results.Compound}");
                        Console.WriteLine("");
                        Console.WriteLine("");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    // Handle the error appropriately
                    ModelState.AddModelError(string.Empty, "An error occurred while generating reviews. Please try again later.");
                    return View(movie);
                }


                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Genre,ReleaseYear,ImdbHyperlink")] Movie movie, IFormFile? poster)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (poster != null && poster.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await poster.CopyToAsync(memoryStream);
                    movie.Poster = memoryStream.ToArray();
                }
                else
                {
                    var existingMovie = await _context.Movie.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
                    movie.Poster = existingMovie?.Poster;
                }

                try
                { 
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.ID))
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
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }


        // GET: Movie/GetMoviePhoto/5
        public async Task<IActionResult> GetMoviePoster(int id)
        {
            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }
            var imageData = movie.Poster;

            return File(imageData, "image/jpg");
        }
    }
}
