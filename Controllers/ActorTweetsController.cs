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
    public class ActorTweetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActorTweetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActorTweets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ActorTweet.Include(a => a.Actor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ActorTweets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorTweet = await _context.ActorTweet
                .Include(a => a.Actor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (actorTweet == null)
            {
                return NotFound();
            }

            return View(actorTweet);
        }

        // GET: ActorTweets/Create
        public IActionResult Create()
        {
            ViewData["ActorID"] = new SelectList(_context.Actor, "ID", "Name");
            return View();
        }

        // POST: ActorTweets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorID,ID,Text,Sentiment")] ActorTweet actorTweet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actorTweet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActorID"] = new SelectList(_context.Actor, "ID", "Name", actorTweet.ActorID);
            return View(actorTweet);
        }

        // GET: ActorTweets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorTweet = await _context.ActorTweet.FindAsync(id);
            if (actorTweet == null)
            {
                return NotFound();
            }
            ViewData["ActorID"] = new SelectList(_context.Actor, "ID", "Name", actorTweet.ActorID);
            return View(actorTweet);
        }

        // POST: ActorTweets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorID,ID,Text,Sentiment")] ActorTweet actorTweet)
        {
            if (id != actorTweet.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actorTweet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorTweetExists(actorTweet.ID))
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
            ViewData["ActorID"] = new SelectList(_context.Actor, "ID", "Name", actorTweet.ActorID);
            return View(actorTweet);
        }

        // GET: ActorTweets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actorTweet = await _context.ActorTweet
                .Include(a => a.Actor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (actorTweet == null)
            {
                return NotFound();
            }

            return View(actorTweet);
        }

        // POST: ActorTweets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorTweet = await _context.ActorTweet.FindAsync(id);
            if (actorTweet != null)
            {
                _context.ActorTweet.Remove(actorTweet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorTweetExists(int id)
        {
            return _context.ActorTweet.Any(e => e.ID == id);
        }
    }
}
