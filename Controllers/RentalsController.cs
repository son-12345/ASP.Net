using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComicSystem.Models;
using ComicSystem.Data;

namespace ComicSystem.Controllers
{
    [Route("api/Rentals")]
    [ApiController]
    public class RentalApiController : ControllerBase
    {
        private readonly ComicSystemContext _context;

        public RentalApiController(ComicSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            return await _context.Rentals
                                 .Include(r => r.Customer)
                                 .Include(r => r.RentalDetails)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rentals
                                       .Include(r => r.Customer)
                                       .Include(r => r.RentalDetails)
                                       .FirstOrDefaultAsync(r => r.RentalID == id);
            if (rental == null) return NotFound();
            return rental;
        }

        [HttpPost]
        public async Task<ActionResult<Rental>> PostRental(Rental rental)
        {
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRental), new { id = rental.RentalID }, rental);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRental(int id, Rental rental)
        {
            if (id != rental.RentalID) return BadRequest();
            _context.Entry(rental).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null) return NotFound();
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RentalExists(int id) => _context.Rentals.Any(e => e.RentalID == id);
    }
}
