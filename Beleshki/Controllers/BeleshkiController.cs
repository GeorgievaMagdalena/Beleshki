using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Beleshki.Models;
using Beleshki.ViewModels;

namespace Beleshki.Controllers
{
    public class BeleshkiController : Controller
    {
        private readonly BeleshkiContext _context;

        public BeleshkiController(BeleshkiContext context)
        {
            _context = context;
        }

        // GET: Beleshki
        public async Task<IActionResult> Index(string searchPredmet, string searchFakultet, string searchIme)
        {
            //var beleshki = _context.Beleshka
              //.ToList();

            var beleshkiQuery = _context.Beleshka
                .Include(b => b.Predmet)
                .ThenInclude(p => p.predmetFakulteti)
                .ThenInclude(pf => pf.Fakultet)
                .AsQueryable();

            var fakultetQuery = _context.Beleshka
               .Where(b => b.Predmet != null && b.Predmet.predmetFakulteti != null)
               .SelectMany(b => b.Predmet.predmetFakulteti)
                .Where(pf => pf.Fakultet != null)
                 .Select(pf => pf.Fakultet.FakultetIme)
                  .Distinct();

            if (!string.IsNullOrEmpty(searchPredmet))
            {
                beleshkiQuery = beleshkiQuery.Where(s => s.Predmet != null && s.Predmet.PredmetIme.Contains(searchPredmet));
            }

            /*if (!string.IsNullOrEmpty(searchPredmet))
            {
                beleshki = (List<Beleshka>)beleshki.Where(s => s.BeleshkaIme != " " && s.BeleshkaIme.Contains(searchIme));
            }*/

            if (!string.IsNullOrEmpty(searchFakultet))
            {
                beleshkiQuery = beleshkiQuery.Where(s => s.Predmet != null &&
                    s.Predmet.predmetFakulteti.Any(pf => pf.Fakultet != null && pf.Fakultet.FakultetIme.Contains(searchFakultet)));
            }

            var beleshkiFakultetVM = new BeleshkaFakultet
            {
                Fakulteti = new SelectList(await fakultetQuery.ToListAsync()),
                Beleshkii = await beleshkiQuery.ToListAsync(),
               // Iminja = beleshki
            };

            return View(beleshkiFakultetVM);
        }


        // GET: Beleshki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Beleshka == null)
            {
                return NotFound();
            }

            var beleshka = await _context.Beleshka
                .Include(b => b.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);

            var komentari = await _context.Komentar
               .Where(r => r.BeleshkaId == id)
               .Select(r => r.komentar)
               .ToListAsync();

            ViewBag.Komentari = komentari;

            if (beleshka == null)
            {
                return NotFound();
            }

            return View(beleshka);
        }

        // GET: Beleshki/Create
        public IActionResult Create()
        {
            ViewData["PredmetId"] = new SelectList(_context.Set<Predmet>(), "Id", "PredmetIme");
            return View();
        }

        // POST: Beleshki/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BeleshkaIme,DatumKreiranje,Opis,DownloadUrl,PredmetId")] Beleshka beleshka, IFormFile file1)
        {
            if (ModelState.IsValid)
            {

                if (file1 != null && file1.Length > 0)
                {
                    var fileName1 = Path.GetFileName(file1.FileName);
                    var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName1);

                    using (var stream = new FileStream(filePath1, FileMode.Create))
                    {
                        await file1.CopyToAsync(stream);
                    }

                    beleshka.DownloadUrl = "./Beleshki/UploadedFiles/" + fileName1;

                }
                    _context.Add(beleshka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PredmetId"] = new SelectList(_context.Set<Predmet>(), "Id", "Id", beleshka.PredmetId);
            return View(beleshka);
        }

        // GET: Beleshki/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Beleshka == null)
            {
                return NotFound();
            }

            var beleshka = await _context.Beleshka.FindAsync(id);
            if (beleshka == null)
            {
                return NotFound();
            }
            ViewData["PredmetId"] = new SelectList(_context.Set<Predmet>(), "Id", "PredmetIme", beleshka.PredmetId);
            return View(beleshka);
        }

        // POST: Beleshki/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BeleshkaIme,DatumKreiranje,Opis,DownloadUrl,PredmetId")] Beleshka beleshka, IFormFile file1)
        {
            if (id != beleshka.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (file1 != null && file1.Length > 0)
                {
                    var fileName1 = Path.GetFileName(file1.FileName);
                    var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName1);

                    using (var stream = new FileStream(filePath1, FileMode.Create))
                    {
                        await file1.CopyToAsync(stream);
                    }

                    var existingBeleshka = await _context.Beleshka.FindAsync(id);

                    if (existingBeleshka != null)
                    {
                        existingBeleshka.BeleshkaIme = beleshka.BeleshkaIme;
                        existingBeleshka.DatumKreiranje = beleshka.DatumKreiranje;
                        existingBeleshka.Opis = beleshka.Opis;
                        existingBeleshka.DownloadUrl = "../UploadedFiles/" + fileName1;
                        existingBeleshka.PredmetId = beleshka.PredmetId;


                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PredmetId"] = new SelectList(_context.Set<Predmet>(), "Id", "PredmetIme", beleshka.PredmetId);
            return View(beleshka);
        }

        // GET: Beleshki/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Beleshka == null)
            {
                return NotFound();
            }

            var beleshka = await _context.Beleshka
                .Include(b => b.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (beleshka == null)
            {
                return NotFound();
            }

            return View(beleshka);
        }

        // POST: Beleshki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Beleshka == null)
            {
                return Problem("Entity set 'BeleshkiContext.Beleshka'  is null.");
            }
            var beleshka = await _context.Beleshka.FindAsync(id);
            if (beleshka != null)
            {
                _context.Beleshka.Remove(beleshka);
            }
            
            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Index));
        }

        private bool BeleshkaExists(int id)
        {
          return (_context.Beleshka?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
