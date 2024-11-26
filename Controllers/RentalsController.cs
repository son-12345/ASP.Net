using ComicSystem.Data;
using ComicSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicSystem.Controllers{
    public class RentalsController : Controller
{
    private readonly ComicSystemContext _context;

    public RentalsController(ComicSystemContext context)
    {
        _context = context;
    }

    // Hiển thị danh sách phiếu thuê
    public async Task<IActionResult> Index()
    {
        var rentals = _context.Rentals.Include(r => r.Customer).ToListAsync();
        return View(await rentals);
    }

    // Form tạo phiếu thuê mới
    public IActionResult Create()
    {
        ViewBag.Customers = _context.Customers.ToList();
        ViewBag.ComicBooks = _context.ComicBooks.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Rental rental, List<int> comicBookIds, List<int> quantities)
    {
        if (ModelState.IsValid)
        {
            rental.RentalDate = DateTime.Now;
            rental.Status = "Active";
            _context.Add(rental);
            await _context.SaveChangesAsync();

            // Lưu chi tiết phiếu thuê
            for (int i = 0; i < comicBookIds.Count; i++)
            {
                var rentalDetail = new RentalDetail
                {
                    RentalID = rental.RentalID,
                    ComicBookID = comicBookIds[i],
                    Quantity = quantities[i],
                    PricePerDay = _context.ComicBooks.First(c => c.ComicBookID == comicBookIds[i]).PricePerDay
                };
                _context.RentalDetails.Add(rentalDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Customers = _context.Customers.ToList();
        ViewBag.ComicBooks = _context.ComicBooks.ToList();
        return View(rental);
    }

    // Hiển thị chi tiết phiếu thuê
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Rentals == null)
            return NotFound();

        var rental = await _context.Rentals
            .Include(r => r.Customer)
            .Include(r => r.RentalDetails)
                .ThenInclude(rd => rd.ComicBook)
            .FirstOrDefaultAsync(m => m.RentalID == id);

        return rental == null ? NotFound() : View(rental);
    }

    // Kết thúc phiếu thuê
    public async Task<IActionResult> Complete(int? id)
    {
        if (id == null || _context.Rentals == null)
            return NotFound();

        var rental = await _context.Rentals.FindAsync(id);
        if (rental == null)
            return NotFound();

        rental.Status = "Completed";
        rental.ReturnDate = DateTime.Now;

        _context.Update(rental);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // Xóa phiếu thuê
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Rentals == null)
            return NotFound();

        var rental = await _context.Rentals.FirstOrDefaultAsync(m => m.RentalID == id);
        return rental == null ? NotFound() : View(rental);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Rentals == null) return Problem("Entity set 'Rentals' is null.");

        var rental = await _context.Rentals.FindAsync(id);
        if (rental != null) _context.Rentals.Remove(rental);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

}