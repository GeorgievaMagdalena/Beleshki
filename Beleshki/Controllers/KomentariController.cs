using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Beleshki.Models;

namespace Beleshki.Controllers
{
    public class KomentariController : Controller
    {
        private readonly BeleshkiContext _context;

        public KomentariController(BeleshkiContext context)
        {
            _context = context;
        }

        // GET: Komentari
        public async Task<IActionResult> Index()
        {
            var beleshkiContext = _context.Komentar.Include(k => k.Beleshka);
            return View(await beleshkiContext.ToListAsync());
        }

        // GET: Komentari/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Komentar == null)
            {
                return NotFound();
            }

            var komentar = await _context.Komentar
                .Include(k => k.Beleshka)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // GET: Komentari/Create
        public IActionResult Create()
        {
            ViewData["BeleshkaId"] = new SelectList(_context.Beleshka, "Id", "BeleshkaIme");
            return View();
        }

        // POST: Komentari/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentIme,komentar,Ocenka,BeleshkaId")] Komentar komentar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(komentar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BeleshkaId"] = new SelectList(_context.Beleshka, "Id", "BeleshkaIme", komentar.BeleshkaId);
            return View(komentar);
        }

        // GET: Komentari/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Komentar == null)
            {
                return NotFound();
            }

            var komentar = await _context.Komentar.FindAsync(id);
            if (komentar == null)
            {
                return NotFound();
            }
            ViewData["BeleshkaId"] = new SelectList(_context.Beleshka, "Id", "BeleshkaIme", komentar.BeleshkaId);
            return View(komentar);
        }

        // POST: Komentari/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentIme,komentar,Ocenka,BeleshkaId")] Komentar komentar)
        {
            if (id != komentar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(komentar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KomentarExists(komentar.Id))
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
            ViewData["BeleshkaId"] = new SelectList(_context.Beleshka, "Id", "BeleshkaIme", komentar.BeleshkaId);
            return View(komentar);
        }

        // GET: Komentari/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Komentar == null)
            {
                return NotFound();
            }

            var komentar = await _context.Komentar
                .Include(k => k.Beleshka)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // POST: Komentari/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Komentar == null)
            {
                return Problem("Entity set 'BeleshkiContext.Komentar'  is null.");
            }
            var komentar = await _context.Komentar.FindAsync(id);
            if (komentar != null)
            {
                _context.Komentar.Remove(komentar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KomentarExists(int id)
        {
          return (_context.Komentar?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
