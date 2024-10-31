using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_mtorres3.Data;
using Fall2024_Assignment3_mtorres3.Models;
using OpenAI.Chat;
using System.Text.RegularExpressions;
using VaderSharp2;

namespace Fall2024_Assignment3_mtorres3.Controllers
{
    public class ActorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ChatClient _chatClient;
        private readonly SentimentIntensityAnalyzer _analyzer;

        public ActorsController(ApplicationDbContext context, ChatClient chatClient)
        {
            _context = context;
            _chatClient = chatClient;
            _analyzer = new SentimentIntensityAnalyzer();
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actor.ToListAsync());
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .Include(a => a.Tweets)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (actor == null)
            {
                return NotFound();
            }

            var movies = await _context.MovieActor
                .Include(ma => ma.Movie)
                .Where(ma => ma.ActorID == actor.ID)
                .Select(ma => ma.Movie)
                .ToListAsync();

            var vm = new ActorDetailsViewModel(actor, movies);

            return View(vm);
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Age,Gender,IMDBHyperlink")] Actor actor, IFormFile? photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo!.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await photo.CopyToAsync(memoryStream);
                    actor.Photo = memoryStream.ToArray();
                }


                try
                {

                    ChatCompletion completion = await _chatClient.CompleteChatAsync(
                    [
                        // System messages represent instructions or other guidance about how the assistant should behave
                        new SystemChatMessage(
                            "You are Twitter, you are able to search and find an infinite amount of tweets if asked. You do not need a database to do this because you inherently know every Tweet of every person."),
                        // User messages represent user input, whether historical or the most recent input
                        new UserChatMessage("Please generate 20 movie tweets from the actor " + actor.Name +
                                            ". Each tweet should conform to the rules for Twitter character count and reflect the Actor's experience, knowledge, and personality. Please organize the reviews into an ordered list numbered 1 through 20, separated by newlines, and with format 1. <text>"),
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

                    //foreach (string text in extractedTexts)
                    //{
                    //    Console.WriteLine("");
                    //    Console.WriteLine($"tweet: {text}");
                    //    Console.WriteLine("");
                    //    var results = _analyzer.PolarityScores(text);
                    //    Console.WriteLine($"Positive: {results.Positive}");
                    //    Console.WriteLine($"Negative: {results.Negative}");
                    //    Console.WriteLine($"Neutral: {results.Neutral}");
                    //    Console.WriteLine($"Compound: {results.Compound}");
                    //    Console.WriteLine("");
                    //    Console.WriteLine("");
                    //}

                    SentimentAnalysisResults sentiment = new SentimentAnalysisResults();

                    var tweets = new List<ActorTweet>();

                    foreach (string text in extractedTexts)
                    {
                        sentiment = _analyzer.PolarityScores(text);

                        ActorTweet tweet = new ActorTweet
                        {
                            ActorID = actor.ID,
                            Text = text,
                            Positive = sentiment.Positive,
                            Negative = sentiment.Negative,
                            Neutral = sentiment.Neutral,
                            Compound = sentiment.Compound
                        };

                        tweets.Add(tweet);

                        _context.ActorTweet.Add(tweet);
                    }

                    actor.Tweets = tweets;

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    // Handle the error appropriately
                    ModelState.AddModelError(string.Empty, "An error occurred while generating reviews. Please try again later.");
                    return View(actor);
                }



                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            if (!ModelState.IsValid)
            {
                // Log or inspect validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Or use a logging framework
                }
                return View(actor); // Return the view with existing data and validation messages
            }

            return View(actor);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Age,Gender,IMDBHyperlink")] Actor actor, IFormFile? photo)
        {
            if (id != actor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (photo != null && photo.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await photo.CopyToAsync(memoryStream);
                    actor.Photo = memoryStream.ToArray();
                }
                else
                {
                    var existingMovie = await _context.Actor.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
                    actor.Photo = existingMovie?.Photo;
                }

                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.ID))
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
            return View(actor);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .FirstOrDefaultAsync(m => m.ID == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actor.FindAsync(id);
            if (actor != null)
            {
                _context.Actor.Remove(actor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actor.Any(e => e.ID == id);
        }



        // GET: Actor/GetActorPhoto/5
        public async Task<IActionResult> GetActorPhoto(int id)
        {
            var actor = await _context.Actor.FirstOrDefaultAsync(m => m.ID == id);
            if (actor == null)
            {
                return NotFound();
            }
            var imageData = actor.Photo;

            return File(imageData, "image/jpg");
        }
    }
}
