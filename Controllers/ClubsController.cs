using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalPro.Data;
using FinalPro.Models.ViewModels;
using FinalPro.Models;
using Microsoft.AspNetCore.Authorization;
namespace FinalPro.Controllers
{
    [Authorize]

    public class ClubsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClubsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> cards()

        {
            ViewData["Title"] = "Clubs List";
            return View(await _context.Club.ToListAsync());
        }
        [HttpGet]
        public IActionResult FindBy()
        {
            ViewData["Title"] = "Find By";
            return View(new SearchClub());
        }

        [HttpPost]
        public async Task<IActionResult> FindBy(SearchClub model)
        {
            ViewData["Title"] = "Find By";
            if (ModelState.IsValid)
            {
                List<Club> clubs = await _context.Club
                    .Where(t => t.Name!.Contains(model.ClubName)).ToListAsync();
                model.Clubs = clubs;
            }
            return View(model);
        }
        public async Task<IActionResult> ContestsReport(int id = -1)
        {
            ViewData["Title"] = "Club Contests";
            ViewData["SubTitle"] = "Details";
            Club? club = null;
            if (id == -1)
            {
                club = await _context.Club.Include(d => d.Contests)!.ThenInclude(t => t.Student).FirstOrDefaultAsync();
            }
            else
            {
                club = await _context.Club.Include(d => d.Contests)!.ThenInclude(t => t.Student).FirstOrDefaultAsync(d => d.ClubId == id);
            }
            if (club == null)
            {
                return RedirectToAction(nameof(Index));
            }


            return View(club);
        }

        // GET: Clubs

        public async Task<IActionResult> Index()

        {
            ViewData["Title"] = "Clubs List";
            return View(await _context.Club.ToListAsync()  );
        }

        // GET: Clubs/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            ViewData["Title"] = "Club Details";
            ViewData["SubTitle"] = "Club Students";

            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Club
                .Include(c => c.Contests)!
                .ThenInclude(s => s.Student)
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // GET: Clubs/Create

        public IActionResult Create()
        {
            ViewData["Title"] = "Create New Club";

            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("ClubId,Name,StartDate")] Club club)
        {
            ViewData["Title"] = "Create New Club";

            if (ModelState.IsValid)
            {
                _context.Add(club);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(club);
        }

        // GET: Clubs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Title"] = "Edit Club";

            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Club.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClubId,Name,StartDate")] Club club)
        {
            ViewData["Title"] = "Edit Club";

            if (id != club.ClubId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(club);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.ClubId))
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
            return View(club);
        }

        // GET: Clubs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["Title"] = "Delete Club";

            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Club
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewData["Title"] = "Delete Club";

            var club = await _context.Club.FindAsync(id);
            if (club != null)
            {
                _context.Club.Remove(club);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubExists(int id)
        {
            return _context.Club.Any(e => e.ClubId == id);
        }
    }
}
