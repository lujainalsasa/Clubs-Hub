using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalPro.Data;
using FinalPro.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace FinalPro.Controllers
{

    public class ContestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contests
        public async Task<IActionResult> Index(string sortby = "Asc")
        {
            ViewData["Title"] = "Contests List";

            List<Contest> contests = await _context.Contest.Include(c => c.Club).Include(c => c.Student).ToListAsync();
            switch (sortby)
            {
                case "Asc":
                    contests = contests.OrderBy(c => c.Name).ToList();
                    ViewData["SortBy"] = "Desc";
                    break;
                case "Desc":
                    contests = contests.OrderByDescending(c => c.Name).ToList();
                    ViewData["SortBy"] = "Asc";
                    break;
            }
            return View(contests);
        }

        // GET: Contests/Details/5
        [Authorize]

        public async Task<IActionResult> Details(int? id)
        {
            ViewData["Title"] = "Contest Details";

            if (id == null)
            {
                return NotFound();
            }

            var contest = await _context.Contest
                .Include(c => c.Club)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.ContestId == id);
            if (contest == null)
            {
                return NotFound();
            }

            return View(contest);
        }

        // GET: Contests/Create
        [Authorize]

        public IActionResult Create()
        {
            ViewData["Title"] = "Create Contest";

            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "Name");
            ViewData["StudentId"] = new SelectList(_context.Student, "StudentId", "FirstName");
            return View();
        }

        // POST: Contests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Create([Bind("ContestId,ClubId,StudentId,Name,Description,Role")] Contest contest)
        {
            ViewData["Title"] = "Create Contest";

            if (ModelState.IsValid)
            {
                _context.Add(contest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "Name", contest.ClubId);
            ViewData["StudentId"] = new SelectList(_context.Student, "StudentId", "StudentId", contest.StudentId);
            return View(contest);
        }

        // GET: Contests/Edit/5
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Title"] = "Edit Contest";

            if (id == null)
            {
                return NotFound();
            }

            var contest = await _context.Contest.FindAsync(id);
            if (contest == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "Name", contest.ClubId);
            ViewData["StudentId"] = new SelectList(_context.Student, "StudentId", "FirstName", contest.StudentId);
            return View(contest);
        }

        // POST: Contests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Edit(int id, [Bind("ContestId,ClubId,StudentId,Name,Description,Role")] Contest contest)
        {
            ViewData["Title"] = "Edit Contest";

            if (id != contest.ContestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContestExists(contest.ContestId))
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
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "Name", contest.ClubId);
            ViewData["StudentId"] = new SelectList(_context.Student, "StudentId", "StudentId", contest.StudentId);
            return View(contest);
        }

        // GET: Contests/Delete/5
        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["Title"] = "Delet Contest";

            if (id == null)
            {
                return NotFound();
            }

            var contest = await _context.Contest
                .Include(c => c.Club)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.ContestId == id);
            if (contest == null)
            {
                return NotFound();
            }

            return View(contest);
        }

        // POST: Contests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewData["Title"] = "Delet Contest";

            var contest = await _context.Contest.FindAsync(id);
            if (contest != null)
            {
                _context.Contest.Remove(contest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContestExists(int id)
        {
            return _context.Contest.Any(e => e.ContestId == id);
        }
    }
}
