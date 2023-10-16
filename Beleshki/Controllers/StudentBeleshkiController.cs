using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Beleshki.Models;
using System.Security.Claims;

namespace Beleshki.Controllers
{
    public class StudentBeleshkiController : Controller
    {
        private readonly BeleshkiContext _context;

        public StudentBeleshkiController(BeleshkiContext context)
        {
            _context = context;
        }

        // GET: StudentBeleshki
        public async Task<IActionResult> Index()
        {
            string userEmail = User.FindFirstValue(ClaimTypes.Email);
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin)
            {
                var studentBeleshki = await _context.StudentBeleshki
                    .Include(u => u.Beleshka)
                    .Where(u => u.StudentIme == userEmail)
                    .ToListAsync();
                return View(studentBeleshki);
            }
            else
            {
                var studentBeleshki = await _context.StudentBeleshki
                    .Include(u => u.Beleshka)
                    .ToListAsync();
                return View(studentBeleshki);
            }
        }
        
        // GET: UserBooks/BuyBook
        public async Task<IActionResult> DodadiTvoi(int? beleshkaId)
        {
            if (beleshkaId == null & _context.StudentBeleshki == null)
            {
                return NotFound();
            }

            var beleshka = await _context.Beleshka.FindAsync(beleshkaId);
            if (beleshka == null)
            {
                return NotFound();
            }

            var signedUser = User.FindFirstValue(ClaimTypes.Email);
            var stBeleshka = new StudentBeleshki
            {
                StudentIme = signedUser,
                BeleshkaId = beleshka.Id
            };

            _context.StudentBeleshki.Add(stBeleshka);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: StudentBeleshki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StudentBeleshki == null)
            {
                return NotFound();
            }

            var studentBeleshki = await _context.StudentBeleshki
                .Include(s => s.Beleshka)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentBeleshki == null)
            {
                return NotFound();
            }

            return View(studentBeleshki);
        }

        // GET: StudentBeleshki/Create
        public IActionResult Create()
        {
            ViewData["BeleshkaId"] = new SelectList(_context.Beleshka, "Id", "BeleshkaIme");
            return View();
        }

        // POST: StudentBeleshki/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentIme,BeleshkaId")] StudentBeleshki studentBeleshki)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentBeleshki);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BeleshkaId"] = new SelectList(_context.Beleshka, "Id", "BeleshkaIme", studentBeleshki.BeleshkaId);
            return View(studentBeleshki);
        }

        // GET: StudentBeleshki/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StudentBeleshki == null)
            {
                return NotFound();
            }

            var studentBeleshki = await _context.StudentBeleshki.FindAsync(id);
            if (studentBeleshki == null)
            {
                return NotFound();
            }
            ViewData["BeleshkaId"] = new SelectList(_context.Beleshka, "Id", "BeleshkaIme", studentBeleshki.BeleshkaId);
            return View(studentBeleshki);
        }

        // POST: StudentBeleshki/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentIme,BeleshkaId")] StudentBeleshki studentBeleshki)
        {
            if (id != studentBeleshki.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentBeleshki);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentBeleshkiExists(studentBeleshki.Id))
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
            ViewData["BeleshkaId"] = new SelectList(_context.Beleshka, "Id", "BeleshkaIme", studentBeleshki.BeleshkaId);
            return View(studentBeleshki);
        }

        // GET: StudentBeleshki/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StudentBeleshki == null)
            {
                return NotFound();
            }

            var studentBeleshki = await _context.StudentBeleshki
                .Include(s => s.Beleshka)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentBeleshki == null)
            {
                return NotFound();
            }

            return View(studentBeleshki);
        }

        // POST: StudentBeleshki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StudentBeleshki == null)
            {
                return Problem("Entity set 'BeleshkiContext.StudentBeleshki'  is null.");
            }
            var studentBeleshki = await _context.StudentBeleshki.FindAsync(id);
            if (studentBeleshki != null)
            {
                _context.StudentBeleshki.Remove(studentBeleshki);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentBeleshkiExists(int id)
        {
          return (_context.StudentBeleshki?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
