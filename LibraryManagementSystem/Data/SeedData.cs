using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

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

                context.Books.AddRange(
                    new Book
                    {
                        Title = "Book 1",
                        Author = "Author 1",
                        Genre = "Genre 1",
                        Available = true
                    },

                    new Book
                    {
                        Title = "Book 2",
                        Author = "Author 2",
                        Genre = "Genre 2",
                        Available = false
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
