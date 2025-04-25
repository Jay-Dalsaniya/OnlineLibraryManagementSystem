using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using SendGrid.Helpers.Mail;
using X.PagedList;
using X.PagedList.Extensions;
using LibraryManagementSystem.Models.Role;
using DocumentFormat.OpenXml.Spreadsheet;


namespace LibraryManagementSystem.Controllers
{
    public class ReaderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;


        public ReaderController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;


        }

        public async Task<IActionResult> Dashboard()
        {
            // Get the UserId of the logged-in user
            var userId = User.FindFirst("UserId")?.Value;

            // If UserId is not available in the claims, redirect to login
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Login", "Account");
            }

            // Get total books issued for the logged-in user
            var totalBooksIssued = await _context.BookRentals
                .Where(br => br.UserId == Convert.ToInt32(userId) && br.RentalStatus == "Active")
                .CountAsync();

            // Get total books returned for the logged-in user
            var totalBooksReturned = await _context.BookRentals
                .Where(br => br.UserId == Convert.ToInt32(userId) && br.RentalStatus == "Returned")
                .CountAsync();

            // Get total books purchased by the logged-in user
            var totalBooksPurchased = await _context.BookPurchases
                .Where(bp => bp.UserId == Convert.ToInt32(userId))
                .CountAsync();

            // Create a ViewModel with the filtered data
            var dashboardData = new ReaderDashboardViewModel
            {
                TotalBooksIssued = totalBooksIssued,
                TotalBooksReturned = totalBooksReturned,
                TotalBooksPurchased = totalBooksPurchased
            };

            return View(dashboardData);
        }

        public async Task<IActionResult> ViewBooks(string searchTerm)
        {
            var booksQuery = _context.Books.Where(b => b.AvailableCopies > 0);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                booksQuery = booksQuery.Where(b =>
                    b.Title.Contains(searchTerm) ||
                    b.Author.Contains(searchTerm) ||
                    b.ISBN.Contains(searchTerm));
            }

            var books = await booksQuery.ToListAsync();
            ViewData["SearchTerm"] = searchTerm;

            return View(books);
        }

        public async Task<IActionResult> IssueBook(int bookId)
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("ViewBooks");
            }

            var book = await _context.Books.FindAsync(bookId);

            if (book == null || book.AvailableCopies <= 0)
            {
                TempData["ErrorMessage"] = "This book is not available for issuing.";
                return RedirectToAction("ViewBooks");
            }

            var rental = new BookRental
            {
                BookId = bookId,
                RentDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(7),
                UserId = Convert.ToInt32(userId),
                Author = book.Author,
                Title = book.Title,
                ISBN = book.ISBN,
                Price = book.Price,
                PublishedDate = book.PublishedDate,
                Genre = book.Genre,
                Category = book.Category,
                Subject = book.Subject,
                BookCondition = book.Condition ?? "Good",
                Summary = book.Summary,
                TotalCopies = book.TotalCopies,
                Language = book.Language,
                Edition = book.Edition,
                RentalStatus = "Active",
            };

            _context.BookRentals.Add(rental);
            book.AvailableCopies--;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Book issued successfully!";
            return RedirectToAction("ViewBooks");
        }

        public IActionResult ViewIssuedBooks()
        {
            var userId = User.FindFirst("UserId")?.Value;  // Use "UserId" to match the claim key

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Dashboard");
            }

            var issuedBooks = _context.BookRentals
                .Where(br => br.UserId.ToString() == userId && br.RentalStatus == "Active")
                .Include(br => br.Book)
                .ToList();

            return View(issuedBooks);
        }
        [HttpGet]
        public async Task<IActionResult> ReturnBook(int rentalId)
        {
            // Fetch the rental record based on rentalId
            var rental = await _context.BookRentals.Include(br => br.Book)
                .FirstOrDefaultAsync(br => br.BookRentalId == rentalId);

            if (rental == null)
            {
                TempData["ErrorMessage"] = "Rental record not found.";
                return RedirectToAction("ViewIssuedBooks");
            }

            // Return the rental object to the view for confirmation
            return View(rental); // Display the confirmation page to the user
        }

        [HttpPost]
        public async Task<IActionResult> ReturnBookConfirmed(int rentalId)
        {
            var rental = await _context.BookRentals
                .Include(br => br.Book)
                .Include(br => br.User)
                .FirstOrDefaultAsync(br => br.BookRentalId == rentalId);

            if (rental == null)
            {
                TempData["ErrorMessage"] = "Rental record not found.";
                return RedirectToAction("ViewIssuedBooks");
            }

            var rentalDate = rental.RentDate;
            var dueDate = rentalDate.AddDays(7);
            var currentDate = DateTime.Now;

            var lateFee = 0m;
            //var fine = 0m;

            if (currentDate > dueDate.AddMinutes(1))
            {
                lateFee = 20m;
                //fine = 20m;
            }

            //var rentalFee = lateFee;

            rental.RentalStatus = "Returned";
            rental.ReturnDate = currentDate;
            rental.LateFee = lateFee;
            //rental.RentalFee = rentalFee;
           // rental.Fine = fine;

            rental.Book.AvailableCopies++;

            await _context.SaveChangesAsync();
            var userEmail = rental.User.Email;
            string subject = "Late Fee Notification";

            string message = $@"
<html>
<head>
    <link href='https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap' rel='stylesheet'>
    <style>
        body {{
            font-family: 'Roboto', sans-serif;
            background-color: #eef2f7;
            color: #444;
            margin: 0;
            padding: 20px;
        }}
        .container {{
            background-color: #ffffff;
            max-width: 600px;
            margin: auto;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 4px 20px rgba(0,0,0,0.1);
        }}
        .header {{
            background: linear-gradient(135deg, #3a7bd5, #00d2ff);
            color: white;
            padding: 30px 20px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            font-size: 24px;
        }}
        .content {{
            padding: 20px 30px;
        }}
        .content p {{
            font-size: 16px;
            line-height: 1.6;
            margin-bottom: 20px;
        }}
        .details {{
            background-color: #f1f7ff;
            padding: 15px;
            border-left: 4px solid #3a7bd5;
            margin-bottom: 20px;
            border-radius: 6px;
        }}
        .details li {{
            margin: 6px 0;
        }}
        .button {{
            display: inline-block;
            background-color: #3a7bd5;
            color: white;
            padding: 12px 25px;
            text-decoration: none;
            border-radius: 30px;
            font-weight: bold;
            box-shadow: 0 2px 8px rgba(0,0,0,0.15);
        }}
        .button:hover {{
            background-color: #2a5fbc;
        }}
        .footer {{
            font-size: 12px;
            text-align: center;
            color: #888;
            padding: 20px;
            background-color: #f9f9f9;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>📚 Late Fee Alert</h1>
        </div>
        <div class='content'>
            <p>Dear <strong>{rental.User.FirstName} {rental.User.LastName}</strong>,</p>
            <p>We noticed that you returned your borrowed book a bit late. A late fee has been applied to your account.</p>

            <div class='details'>
                <ul>
                    <li><strong>📖 Book Title:</strong> {rental.Book.Title}</li>
                    <li><strong>💰 Late Fee:</strong> ₹{lateFee:F2}</li>
                    <li><strong>📅 Return Date:</strong> {currentDate:yyyy-MM-dd HH:mm}</li>
                    <li><strong>📌 Due Date:</strong> {dueDate:yyyy-MM-dd HH:mm}</li>
                </ul>
            </div>

            <p>If you have any questions or think this is a mistake, feel free to reach out to our support team.</p>
            <p style='text-align: center;'>
                <a href='#' class='button'>Contact Support</a>
            </p>
            <p>Thank you for being a valued member of our library! 🌟</p>
        </div>
        <div class='footer'>
            This is an automated message. Please do not reply.
        </div>
    </div>
</body>
</html>";


            await _emailSender.SendEmailAsync(userEmail, subject, message);



            TempData["SuccessMessage"] = lateFee > 0
                ? $"Book returned successfully! Late fee: ₹{lateFee:F2}"
                : "Book returned successfully! No late fee applied.";

            return RedirectToAction("ViewIssuedBooks");
        }

        [HttpGet]
        public async Task<IActionResult> ViewReturnedBooks()
        {
            // Get the current user ID from the logged-in user
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Login", "Account");
            }

            // Fetch returned books for the logged-in user
            var returnedBooks = await _context.BookRentals
                .Include(br => br.Book)
                .Where(br => br.RentalStatus == "Returned" && br.UserId == Convert.ToInt32(userId))
                .Select(br => new
                {
                    br.BookRentalId,
                    br.Book.Title,
                    br.Book.Author,
                    br.Book.ISBN,
                    br.Book.Publisher,
                    br.Book.Genre,
                    br.Book.Price,
                    br.RentDate,
                    br.ReturnDate
                })
                .ToListAsync();

            // Pass the returned books data to the view
            return View(returnedBooks);
        }

        //public async Task<IActionResult> AvailableBooks(string searchTerm = "", decimal? minPrice = null, decimal? maxPrice = null, int page = 1)
        //{
        //    // Query for books ensuring they have available copies
        //    var booksQuery = _context.Books
        //                             .Where(b => b.TotalCopies > 0 && b.AvailableCopies > 0)
        //                             .AsQueryable(); // Ensure it's IQueryable for efficient querying

        //    // Search functionality: Filter by Title or Author
        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        booksQuery = booksQuery.Where(b =>
        //            b.Title.Contains(searchTerm) ||
        //            b.Author.Contains(searchTerm));
        //    }

        //    // Optional price range filtering
        //    if (minPrice.HasValue)
        //    {
        //        booksQuery = booksQuery.Where(b => b.Price >= minPrice.Value);
        //    }

        //    if (maxPrice.HasValue)
        //    {
        //        booksQuery = booksQuery.Where(b => b.Price <= maxPrice.Value);
        //    }

        //    // Fetch paginated results (10 items per page)
        //    var booksList = await booksQuery
        //        .OrderBy(b => b.Title)  // You can order by title, author, or another field
        //        .ToListAsync();         // Execute the query asynchronously

        //    var pagedBooks = booksList.ToPagedList(page, 10); // Paginate the results to 10 per page

        //    // Pass search term and price filters to the view
        //    ViewData["SearchTerm"] = searchTerm;
        //    ViewData["MinPrice"] = minPrice;
        //    ViewData["MaxPrice"] = maxPrice;

        //    return View(pagedBooks);
        //}
        public async Task<IActionResult> AvailableBooks(string searchTerm = "", decimal? minPrice = null, decimal? maxPrice = null, int page = 1)
        {
            // Query for books ensuring they have sold books (SellBook > 0) instead of AvailableCopies
            var booksQuery = _context.Books
                                     .Where(b => b.SellBook > 0) // Change filter from AvailableCopies to SellBook
                                     .AsQueryable(); // Ensure it's IQueryable for efficient querying

            // Search functionality: Filter by Title or Author
            if (!string.IsNullOrEmpty(searchTerm))
            {
                booksQuery = booksQuery.Where(b =>
                    b.Title.Contains(searchTerm) ||
                    b.Author.Contains(searchTerm));
            }

            // Optional price range filtering
            if (minPrice.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.Price <= maxPrice.Value);
            }

            // Fetch paginated results (10 items per page)
            var booksList = await booksQuery
                .OrderBy(b => b.Title)  // You can order by title, author, or another field
                .ToListAsync();         // Execute the query asynchronously

            var pagedBooks = booksList.ToPagedList(page, 10); // Paginate the results to 10 per page

            // Pass search term and price filters to the view
            ViewData["SearchTerm"] = searchTerm;
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;

            return View(pagedBooks);
        }



        public async Task<IActionResult> BuyBook(int bookId)
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("AvailableBooks");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var book = await _context.Books.FindAsync(bookId);

                // Check if the book is available for purchase (SellBook > 0)
                if (book == null || book.SellBook <= 0)
                {
                    TempData["ErrorMessage"] = "This book is no longer available for purchase.";
                    return RedirectToAction("AvailableBooks");
                }

                // Create a new purchase record
                var purchase = new BookPurchase
                {
                    BookId = bookId,
                    UserId = Convert.ToInt32(userId),
                    PurchaseDate = DateTime.Now,
                    Price = book.Price,
                    Title = book.Title,
                    Author = book.Author,
                    ISBN = book.ISBN,
                    Genre = book.Genre,
                    Category = book.Category,
                    Subject = book.Subject,
                    Summary = book.Summary,
                    TotalCopies = book.TotalCopies,
                    Language = book.Language,
                    Edition = book.Edition,
                };

                book.SellBook--;

                _context.BookPurchases.Add(purchase);
                _context.Books.Update(book);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Book purchased successfully!";
                return RedirectToAction("ViewPurchasedBooks");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "An error occurred while processing your purchase.";
                Console.WriteLine(ex.Message);
                return RedirectToAction("AvailableBooks");
            }
        }





        //public async Task<IActionResult> BookDetails(int bookId)
        //{
        //    // Find the book by its ID
        //    var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);

        //    // Check if the book is found
        //    if (book == null)
        //    {
        //        TempData["ErrorMessage"] = "Book not found.";
        //        return RedirectToAction("ViewBooks");
        //    }

        //    // Pass the book details to the view
        //    return View(book);
        //}
        // Ensure this is included for async methods

        public async Task<IActionResult> BookDetails(int bookId)
        {
            // Find the book by its ID asynchronously
            var book = await _context.Books
                                      .FirstOrDefaultAsync(b => b.BookId == bookId);

            // Check if the book is found
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found.";
                return RedirectToAction("ViewBooks");
            }

            // Pass the book details to the view
            return View(book);
        }


        public async Task<IActionResult> ViewPurchasedBooks(string searchTerm = "", decimal? minPrice = null, decimal? maxPrice = null, int page = 1)
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Dashboard");
            }

            // Query to get purchased books
            var purchasedBooksQuery = _context.BookPurchases
                .Where(bp => bp.UserId.ToString() == userId)
                .Include(bp => bp.Book)
                .AsQueryable();

            // Search functionality
            if (!string.IsNullOrEmpty(searchTerm))
            {
                purchasedBooksQuery = purchasedBooksQuery
                    .Where(bp => bp.Book.Title.Contains(searchTerm) ||
                                 bp.Book.Author.Contains(searchTerm) ||
                                 bp.Book.ISBN.Contains(searchTerm));
            }

            // Filter by price range if provided
            if (minPrice.HasValue)
            {
                purchasedBooksQuery = purchasedBooksQuery.Where(bp => bp.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                purchasedBooksQuery = purchasedBooksQuery.Where(bp => bp.Price <= maxPrice.Value);
            }

            // Get paginated results
            var purchasedBooksList = await purchasedBooksQuery
                .OrderBy(bp => bp.PurchaseDate)
                .ToListAsync();

            var purchasedBooksPaged = purchasedBooksList.ToPagedList(page, 10);

            // Pass the search term and price range to the view
            ViewData["SearchTerm"] = searchTerm;
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;

            return View(purchasedBooksPaged);
        }

    }
}
