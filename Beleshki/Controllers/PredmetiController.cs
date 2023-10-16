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
    public class PredmetiController : Controller
    {
        private readonly BeleshkiContext _context;

        public PredmetiController(BeleshkiContext context)
        {
            _context = context;
        }

        // GET: Predmeti
        public async Task<IActionResult> Index(string searchKod)
        {
            IQueryable<Predmet> predmeti = _context.Predmet.AsQueryable();

            if (!string.IsNullOrEmpty(searchKod))
            {
                predmeti = predmeti.Where(s => s.Kod.Contains(searchKod));
            }

            return View(await predmeti.ToListAsync()); ;
        }

        // GET: Predmeti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Predmet == null)
            {
                return NotFound();
            }

            var predmet = await _context.Predmet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (predmet == null)
            {
                return NotFound();
            }

            var beleshki = await _context.Beleshka
                .Where(r => r.PredmetId == id)
                .Select(r => r.BeleshkaIme)
                .ToListAsync();

            ViewBag.Beleshki = beleshki;

            return View(predmet);
        }

        // GET: Predmeti/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Predmeti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PredmetIme,Kod,Krediti,Institut,StudiskaGodina")] Predmet predmet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(predmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(predmet);
        }

        // GET: Predmeti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Predmet == null)
            {
                return NotFound();
            }

            var predmet = await _context.Predmet.FindAsync(id);
            if (predmet == null)
            {
                return NotFound();
            }
            return View(predmet);
        }

        // POST: Predmeti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PredmetIme,Kod,Krediti,Institut,StudiskaGodina")] Predmet predmet)
        {
            if (id != predmet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(predmet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PredmetExists(predmet.Id))
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
            return View(predmet);
        }

        // GET: Predmeti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Predmet == null)
            {
                return NotFound();
            }

            var predmet = await _context.Predmet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (predmet == null)
            {
                return NotFound();
            }

            return View(predmet);
        }

        // POST: Predmeti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Predmet == null)
            {
                return Problem("Entity set 'BeleshkiContext.Predmet'  is null.");
            }
            var predmet = await _context.Predmet.FindAsync(id);
            if (predmet != null)
            {
                _context.Predmet.Remove(predmet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PredmetExists(int id)
        {
          return (_context.Predmet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
