using System.Collections.Generic;
using System.Threading.Tasks;
using BookSearchService.Models;
using BookSearchService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookSearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookSearchServiceHandler _bookSearchServiceHandler;

        public BooksController(BookSearchServiceHandler bookSearchServiceHandler)
        {
            _bookSearchServiceHandler = bookSearchServiceHandler;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Book>>> SearchBooks([FromQuery] string query)
        {
            var books = await _bookSearchServiceHandler.SearchBooksAsync(query);
            return Ok(books);
        }
    }
}
