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
    public class FakultetiController : Controller
    {
        private readonly BeleshkiContext _context;

        public FakultetiController(BeleshkiContext context)
        {
            _context = context;
        }

        // GET: Fakulteti
        public async Task<IActionResult> Index()
        {
              return _context.Fakultet != null ? 
                          View(await _context.Fakultet.ToListAsync()) :
                          Problem("Entity set 'BeleshkiContext.Fakultet'  is null.");
        }

        // GET: Fakulteti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fakultet == null)
            {
                return NotFound();
            }

            var fakultet = await _context.Fakultet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fakultet == null)
            {
                return NotFound();
            }

            return View(fakultet);
        }

        // GET: Fakulteti/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fakulteti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FakultetIme,UniverzitetIme,LogoURL")] Fakultet fakultet, IFormFile file2)
        {
            if (ModelState.IsValid)
            {
                if (file2 != null && file2.Length > 0)
                {
                    var fileName2 = Path.GetFileName(file2.FileName);
                    var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName2);

                    using (var stream = new FileStream(filePath2, FileMode.Create))
                    {
                        await file2.CopyToAsync(stream);
                    }
                    fakultet.LogoURL = "/UploadedFiles/" + fileName2; 
                }

                _context.Add(fakultet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fakultet);
        }

        // GET: Fakulteti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var fakultet = _context.Fakultet.Where(m => m.Id == id).Include(m => m.predmetiFakultet).FirstOrDefault(); ;

            //var fakultet = await _context.Fakultet.FindAsync(id);
            if (fakultet == null)
            {
                return NotFound();
            }

            var predmeti = _context.Predmet.AsEnumerable();
            predmeti = predmeti.OrderBy(s => s.PredmetIme);

            PredmetFakultetEditViewModel viewmodel = new PredmetFakultetEditViewModel
            {
                Fakultet = (Fakultet)fakultet,
                PredmetiLista = new MultiSelectList(predmeti, "Id", "PredmetIme"),
                SelectedPredmeti = fakultet.predmetiFakultet.Select(sa => sa.PredmetId)
            };

            return View(viewmodel);
        }

        // POST: Fakulteti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PredmetFakultetEditViewModel viewModel, IFormFile file2)
        {
            if (id != viewModel.Fakultet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingFakultet = await _context.Fakultet
                        .Include(f => f.predmetiFakultet) 
                        .FirstOrDefaultAsync(f => f.Id == id);

                    if (existingFakultet == null)
                    {
                        return NotFound();
                    }

                    // Update the Fakultet properties
                    existingFakultet.FakultetIme = viewModel.Fakultet.FakultetIme;
                    existingFakultet.UniverzitetIme = viewModel.Fakultet.UniverzitetIme;

                    // Handle file upload here (if needed)
                    if (file2 != null && file2.Length > 0)
                    {
                        var fileName2 = Path.GetFileName(file2.FileName);
                        var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName2);

                        using (var stream = new FileStream(filePath2, FileMode.Create))
                        {
                            await file2.CopyToAsync(stream);
                        }

                        existingFakultet.LogoURL = "../UploadedFiles/" + fileName2;
                    }

                    if (viewModel.SelectedPredmeti != null)
                    {
                        var toBeRemoved = existingFakultet.predmetiFakultet
                            .Where(pf => !viewModel.SelectedPredmeti.Contains(pf.PredmetId))
                            .ToList();

                        foreach (var predmetFakultet in toBeRemoved)
                        {
                            existingFakultet.predmetiFakultet.Remove(predmetFakultet);
                        }

                        var newPredmetiIds = viewModel.SelectedPredmeti.Except(existingFakultet.predmetiFakultet.Select(pf => pf.PredmetId));
                        foreach (var predmetId in newPredmetiIds)
                        {
                            existingFakultet.predmetiFakultet.Add(new PredmetFakultet { FakultetId = id, PredmetId = predmetId });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FakultetExists(viewModel.Fakultet.Id))
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

            return View(viewModel);
        }


        // GET: Fakulteti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fakultet == null)
            {
                return NotFound();
            }

            var fakultet = await _context.Fakultet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fakultet == null)
            {
                return NotFound();
            }

            return View(fakultet);
        }

        // POST: Fakulteti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fakultet == null)
            {
                return Problem("Entity set 'BeleshkiContext.Fakultet'  is null.");
            }
            var fakultet = await _context.Fakultet.FindAsync(id);
            if (fakultet != null)
            {
                _context.Fakultet.Remove(fakultet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FakultetExists(int id)
        {
          return (_context.Fakultet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
