using ComicSystem.Data;
using ComicSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicSystem.Controllers{
    public class ComicBooksController : Controller
{
    private readonly ComicSystemContext _context;

    public ComicBooksController(ComicSystemContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.ComicBooks.ToListAsync());
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ComicBook comicBook)
    {
        if (ModelState.IsValid)
        {
            _context.Add(comicBook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(comicBook);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.ComicBooks == null)
            return NotFound();

        var comicBook = await _context.ComicBooks.FindAsync(id);
        return comicBook == null ? NotFound() : View(comicBook);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ComicBook comicBook)
    {
        if (id != comicBook.ComicBookID)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(comicBook);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ComicBooks.Any(e => e.ComicBookID == id))
                    return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(comicBook);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.ComicBooks == null)
            return NotFound();

        var comicBook = await _context.ComicBooks.FirstOrDefaultAsync(m => m.ComicBookID == id);
        return comicBook == null ? NotFound() : View(comicBook);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.ComicBooks == null) return Problem("Entity set 'ComicBooks' is null.");

        var comicBook = await _context.ComicBooks.FindAsync(id);
        if (comicBook != null) _context.ComicBooks.Remove(comicBook);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

}