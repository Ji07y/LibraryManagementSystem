using Microsoft.AspNetCore.Mvc;
using BookSearch.Models;
using BookSearch.Data;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly LibraryContext _context;

    public BooksController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Book>>> SearchBooks([FromQuery] string query)
    {
        var books = await _context.Books
                                  .Where(b => b.Title.Contains(query) || b.Author.Contains(query))
                                  .ToListAsync();
        return Ok(books);
    }
}
