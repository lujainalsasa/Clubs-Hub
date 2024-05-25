using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalPro.Data;
using FinalPro.Models;
using Microsoft.AspNetCore.Authorization;

namespace FinalPro.Controllers
{
    [Authorize]

    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        [Route("[Controller]/[Action]")]
        [Route("LujainAndShahd")]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Students List";

            IEnumerable<Student> students = await _context.Student.ToListAsync();
            students = students.Where(s => s.EducationLevel == eEducationLevel.Senior);
            return View(students);
        }
        public async Task<IActionResult> ContestsReport(int id = -1)
        {
            ViewData["Title"] = "Student Contests";
            ViewData["SubTitle"] = "Details";
            Student? student = null;
            if (id == -1)
            {
                student = await _context.Student.Include(d => d.Contests)!.ThenInclude(t => t.Club).FirstOrDefaultAsync();
            }
            else
            {
                student = await _context.Student.Include(d => d.Contests)!.ThenInclude(t => t.Club).FirstOrDefaultAsync(d => d.StudentId == id);
            }
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }
           

            return View(student);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["Title"] = "Student Details";
            ViewData["SubTitle"] = "Student Clubs";

            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s=>s.Contests)
                .ThenInclude(c=>c.Club)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["Title"] = "Create New Student";

            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FirstName,LastName,Email,Phone,EducationLevel")] Student student)
        {
            ViewData["Title"] = "Create New Student";

            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Title"] = "Edit Student";

            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,Email,Phone,EducationLevel")] Student student)
        {
            ViewData["Title"] = "Edit Student";

            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["Title"] = "Delete Student";

            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewData["Title"] = "Delete Student";

            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
