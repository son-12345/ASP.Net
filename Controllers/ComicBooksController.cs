using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComicSystem.Models;
using ComicSystem.Data;

namespace ComicSystem.Controllers
{
    [Route("api/ComicBooks")]
    [ApiController]
    public class ComicBookApiController : ControllerBase
    {
        private readonly ComicSystemContext _context;

        public ComicBookApiController(ComicSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComicBook>>> GetComicBooks()
        {
            return await _context.ComicBooks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComicBook>> GetComicBook(int id)
        {
            var comicBook = await _context.ComicBooks.FindAsync(id);
            if (comicBook == null) return NotFound();
            return comicBook;
        }

        [HttpPost]
        public async Task<ActionResult<ComicBook>> PostComicBook(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetComicBook), new { id = comicBook.ComicBookID }, comicBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComicBook(int id, ComicBook comicBook)
        {
            if (id != comicBook.ComicBookID) return BadRequest();
            _context.Entry(comicBook).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComicBookExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComicBook(int id)
        {
            var comicBook = await _context.ComicBooks.FindAsync(id);
            if (comicBook == null) return NotFound();
            _context.ComicBooks.Remove(comicBook);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ComicBookExists(int id) => _context.ComicBooks.Any(e => e.ComicBookID == id);
    }
}
