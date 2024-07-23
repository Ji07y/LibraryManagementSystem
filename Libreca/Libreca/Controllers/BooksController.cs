using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Libreca.Data;
using Libreca.Models;
using NotificationService;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Libreca.Services;
using System;

namespace Libreca.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;
        private readonly RabbitMQService _rabbitMQService;
        private readonly BookSearchClient _bookSearchClient;

        public BooksController(LibraryContext context, RabbitMQService rabbitMQService, BookSearchClient bookSearchClient)
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
            _bookSearchClient = bookSearchClient;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }
     
        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            var books = await _bookSearchClient.SearchBooksAsync(query);
            return View(books);
        }


        [HttpPost]
        public async Task<IActionResult> Loan(int BookId, string User)
        {
            var loan = new Loan
            {
                BookId = BookId,
                User = User,
                LoanDate = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                _context.Loans.Add(loan);

                var book = await _context.Books.FindAsync(BookId);
                if (book != null)
                {
                    book.Available = false;
                    _context.Update(book);
                }

                await _context.SaveChangesAsync();
                _rabbitMQService.SendEmail("carlosochoa@gmail.com", "Hola", $"Has rentado este libro: {book.Title}");
                return RedirectToAction(nameof(Index));
            }
            return View(loan);
        }

        [HttpPost]
        public async Task<IActionResult> Return(int BookId)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.BookId == BookId && l.ReturnDate == null);
            if (loan == null)
            {
                return NotFound();
            }

            loan.ReturnDate = DateTime.Now;
            loan.Book.Available = true;
            _context.Update(loan);
            await _context.SaveChangesAsync();
            _rabbitMQService.SendEmail("carlosochoa@gmail.com", "Hola", $"Has devuelto este libro: { loan.Book.Title}");
            return RedirectToAction(nameof(Index));
        }
    }
}
