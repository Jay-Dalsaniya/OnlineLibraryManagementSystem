using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class BookRentalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookRentalController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult IssueBook(int bookId, int userId)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (book == null || user == null)
            {
                TempData["ErrorMessage"] = "Book or User not found.";
                return RedirectToAction("Index", "Book");
            }

            if (book.AvailableCopies <= 0)
            {
                TempData["ErrorMessage"] = "No copies available for this book.";
                return RedirectToAction("Index", "Book");
            }
            var rental = new BookRental
            {
                BookId = book.BookId,
                UserId = user.Id,
                RentDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),  
                RentalStatus = "Active",  
                BookCondition = "Good", 
                CreatedDate = DateTime.Now,
                Title = book.Title,  
                Author = book.Author,  
                ISBN = book.ISBN,  
                Price = book.Price, 
                PublishedDate = book.PublishedDate, 
                Genre = book.Genre, 
                Category = book.Category, 
                Subject = book.Subject, 
                Summary = book.Summary, 
                TotalCopies = book.TotalCopies, 
                Language = book.Language,  
                Edition = book.Edition,  
                //RentalFee = 20.00m, 
                //Fine = 0,  
            };

            _context.BookRentals.Add(rental);
            book.AvailableCopies--;  
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Book issued successfully.";
            return RedirectToAction("Index", "Book");
        }

        
        public IActionResult ReturnBook(int id)
        {
            var rental = _context.BookRentals.FirstOrDefault(r => r.BookRentalId == id);
            if (rental == null)
            {
                TempData["ErrorMessage"] = "Rental not found.";
                return RedirectToAction("Index", "Book");
            }

            var overdueMinutes = (DateTime.Now - rental.DueDate).TotalMinutes;

            decimal lateFee = 0;
            if (overdueMinutes > 0)
            {
                lateFee = CalculateLateFee(overdueMinutes);
            }

            rental.LateFee = lateFee;
            rental.ReturnDate = DateTime.Now;
            rental.RentalStatus = "Returned";  

            var book = _context.Books.FirstOrDefault(b => b.BookId == rental.BookId);
            if (book != null)
            {
                book.AvailableCopies++;
            }

            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Book returned successfully. Late fee: ₹{lateFee}.";
            return RedirectToAction("Index", "Book");
        }

        private decimal CalculateLateFee(double overdueMinutes)
        {
            decimal lateFee = 0;

            if (overdueMinutes > 0)
            {
                lateFee = (decimal)(Math.Ceiling(overdueMinutes) * 20);  
            }

            return lateFee;
        }

        private decimal CalculateLateFee(int overdueDays)
        {
            decimal lateFee = 0;

           
            if (overdueDays > 0)
            {
                lateFee = overdueDays * 50;  
            }

            return lateFee;
        }
    }
}
