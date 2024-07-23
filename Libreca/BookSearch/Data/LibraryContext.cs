using BookSearch.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
    }
}
