using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookSearchService.Data;
using BookSearchService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookSearchService.Services
{
    public class BookSearchServiceHandler
    {
        private readonly LibraryContext _context;

        public BookSearchServiceHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> SearchBooksAsync(string query)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(query) || b.Author.Contains(query) || b.Genre.Contains(query))
                .ToListAsync();
        }
    }
}
