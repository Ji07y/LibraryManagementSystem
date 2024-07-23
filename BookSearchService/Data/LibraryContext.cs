using Microsoft.EntityFrameworkCore;
using BookSearchService.Models;

namespace BookSearchService.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
    }
}
