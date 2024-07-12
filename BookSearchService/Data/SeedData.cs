using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace LibraryManagementSystem.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LibraryContext(
                serviceProvider.GetRequiredService<DbContextOptions<LibraryContext>>()))
            {
                // Look for any books.
                if (context.Books.Any())
                {
                    return;   // DB has been seeded
                }

                // Ensure using the correct namespace for Book
                var libraryBooks = new LibraryManagementSystem.Models.Book[]
                {
                    new LibraryManagementSystem.Models.Book
                    {
                        Title = "Book 1",
                        Author = "Author 1",
                        Genre = "Genre 1",
                        Available = true
                    },

                    new LibraryManagementSystem.Models.Book
                    {
                        Title = "Book 2",
                        Author = "Author 2",
                        Genre = "Genre 2",
                        Available = false
                    }
                };

                context.Books.AddRange(libraryBooks);
                context.SaveChanges();
            }
        }
    }
}
