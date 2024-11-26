using ComicSystem.Data;
using ComicSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ComicSystem.Controllers{
    public class CustomersController : Controller
{
    private readonly ComicSystemContext _context;

    public CustomersController(ComicSystemContext context)
    {
        _context = context;
    }

    // Hiển thị danh sách khách hàng
    public async Task<IActionResult> Index()
    {
        return View(await _context.Customers.ToListAsync());
    }

    // Form đăng ký khách hàng mới
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(Customer customer)
    {
        if (ModelState.IsValid)
        {
            customer.RegistrationDate = DateTime.Now;
            _context.Add(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // Hiển thị thông tin chi tiết khách hàng
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Customers == null)
            return NotFound();

        var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerID == id);
        return customer == null ? NotFound() : View(customer);
    }

    // Form chỉnh sửa thông tin khách hàng
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Customers == null)
            return NotFound();

        var customer = await _context.Customers.FindAsync(id);
        return customer == null ? NotFound() : View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Customer customer)
    {
        if (id != customer.CustomerID)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Customers.Any(e => e.CustomerID == id))
                    return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // Xóa khách hàng
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Customers == null)
            return NotFound();

        var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerID == id);
        return customer == null ? NotFound() : View(customer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Customers == null) return Problem("Entity set 'Customers' is null.");

        var customer = await _context.Customers.FindAsync(id);
        if (customer != null) _context.Customers.Remove(customer);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

}
