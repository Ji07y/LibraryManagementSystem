using System;
using System.ComponentModel.DataAnnotations;

namespace Libreca.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public string User { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public Book Book { get; set; }
    }
}
