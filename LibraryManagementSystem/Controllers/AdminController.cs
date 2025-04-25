using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.Books;
using LibraryManagementSystem.Models.Role;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using X.PagedList.Extensions;

namespace LibraryManagementSystem.Controllers
{

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public AdminController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;

        }

        // Index Action
        //public async Task<IActionResult> Index()
        //{
        //    var dashboardData = new DashboardViewModel
        //    {
        //        TotalBooks = await _context.Books.CountAsync(),
        //        IssuedBooks = await _context.BookRentals.CountAsync(br => !br.ReturnDate.HasValue),
        //        ReturnedBooks = await _context.BookRentals.CountAsync(br => br.ReturnDate.HasValue),
        //        AvailableBooks = await _context.Books.SumAsync(b => b.TotalCopies) - await _context.BookRentals.CountAsync(br => !br.ReturnDate.HasValue),
        //        TotalAuthors = await _context.Books.Select(b => b.Author).Distinct().CountAsync(),
        //        TotalUsers = await _context.Users.CountAsync(),
        //        TotalSales = await _context.BookPurchases.SumAsync(bp => bp.Price),
        //        TotalBookPurchases = await _context.BookPurchases.CountAsync()
        //    };

        //    return View(dashboardData);
        //}

        public async Task<IActionResult> Index()
        {
            var dashboardData = new DashboardViewModel
            {
                // Basic Data
                TotalBooks = await _context.Books.CountAsync(),
                IssuedBooks = await _context.BookRentals.CountAsync(br => !br.ReturnDate.HasValue),
                ReturnedBooks = await _context.BookRentals.CountAsync(br => br.ReturnDate.HasValue),
                AvailableBooks = await _context.Books.SumAsync(b => b.TotalCopies) - await _context.BookRentals.CountAsync(br => !br.ReturnDate.HasValue),
                TotalAuthors = await _context.Books.Select(b => b.Author).Distinct().CountAsync(),
                TotalUsers = await _context.Users
                            .Where(u => u.Role != "Admin" && u.Role != "Librarian")
                            .CountAsync(),
                // Sales and Purchases Data
                TotalSales = await _context.BookPurchases.SumAsync(bp => bp.Price),
                TotalBooksSold = await _context.BookPurchases.CountAsync(),
                TotalBookPurchases = await _context.BookPurchases.CountAsync(),

                // Additional Metrics
                ActiveTransactions = await _context.BookRentals.CountAsync(br => !br.ReturnDate.HasValue), // Active Rentals
                OverdueBooks = await _context.BookRentals.CountAsync(br => br.ReturnDate.HasValue && br.DueDate < DateTime.Now && !br.ReturnDate.HasValue), // Overdue Rentals
                TotalLateFees = await _context.BookRentals.Where(br => br.LateFee.HasValue).SumAsync(br => br.LateFee.Value), // Sum of all late fees
                TotalReturnedBooks = await _context.BookRentals.CountAsync(br => br.ReturnDate.HasValue), // Books Returned
                TotalBooksInStock = await _context.Books.SumAsync(b => b.TotalCopies) // Total stock in the library
            };

            return View(dashboardData);
        }



        //public IActionResult Index()
        //{
        //    var role = User.FindFirst("Role")?.Value;
        //    Console.WriteLine($"User role: {role}");

        //    var books = _context.Books
        //        .Include(b => b.BookRentals) 
        //        .Include(b => b.BookPurchases) 
        //        .ToList();

        //    var bookDetails = books.Select(b => new AdminBookDetailViewModel
        //    {
        //        BookId = b.BookId,
        //        Title = b.Title,
        //        Author = b.Author,
        //        Publisher = b.Publisher,
        //        ISBN = b.ISBN,
        //        Category = b.Category,
        //        TotalCopies = b.TotalCopies,
        //        IssuedCount = b.BookRentals.Count(r => !r.ReturnDate.HasValue), 
        //        ReturnedCount = b.BookRentals.Count(r => r.ReturnDate.HasValue), 
        //        PurchasedCount = b.BookPurchases.Count(),
        //        AvailableCount = b.TotalCopies - b.BookRentals.Count(r => !r.ReturnDate.HasValue)
        //    }).ToList();

        //    ViewBag.BookDetails = bookDetails;

        //    return View();
        //}

        public IActionResult Dashboard()
        {
            var role = User.FindFirst("Role")?.Value;
            Console.WriteLine($"User role: {role}");

            if (role == "Admin")
            {
                return RedirectToAction("Index", "Admin");

            }
            else
            {
                return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpGet]
        public IActionResult ViewAllUsers()
        {
            var users = _context.Users
                .Where(u => u.Role != "Admin")
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    ImageUrl = u.ImageName
                })
                .ToList();

            return View(users);
        }

        [HttpGet]
        public IActionResult ViewAllBooks()
        {
            var books = _context.Books
                .Include(b => b.BookRentals)
                .Include(b => b.BookPurchases)
                .ToList();

            var bookDetails = books.Select(b => new AdminBookDetailViewModel
            {
                BookId = b.BookId,
                Title = b.Title,
                TotalCopies = b.TotalCopies,
                IssuedCount = b.BookRentals.Count(r => !r.ReturnDate.HasValue),
                ReturnedCount = b.BookRentals.Count(r => r.ReturnDate.HasValue),
                PurchasedCount = b.BookPurchases.Count(),
                AvailableCount = b.TotalCopies - b.BookRentals.Count(r => !r.ReturnDate.HasValue)
            }).ToList();

            return View(bookDetails);
        }

        [HttpGet]
        public IActionResult ViewUserDetails(int userId)
        {
            var user = _context.Users
                .Include(u => u.BookRentals)
                    .ThenInclude(br => br.Book)
                .Include(u => u.BookPurchases)
                    .ThenInclude(bp => bp.Book)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("ViewAllUsers");
            }

            if (user.Role == "Librarian")
            {
                var uploadedBooks = _context.Books
                    .Where(b => b.CreatedBy == user.FirstName)
                    .ToList();

                ViewBag.UploadedBooks = uploadedBooks;
            }

            return View(user);
        }
        [HttpGet]
        public IActionResult ViewBookDetails(int bookId)
        {
            var book = _context.Books
                .Include(b => b.BookRentals)
                    .ThenInclude(br => br.User)
                .Include(b => b.BookPurchases)
                    .ThenInclude(bp => bp.User)
                .FirstOrDefault(b => b.BookId == bookId);

            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found.";
                return RedirectToAction("ViewAllBooks");
            }

            return View(book);
        }

        public IActionResult ManageUsers(string search)
        {
            var users = _context.Users
                .Where(u => u.Role != "Admin")
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search) || u.Email.Contains(search));
            }

            return View(users.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.IsActive = false;
                _context.SaveChanges();

                string subject = "⚠️ Account Deactivated - Library Notification";
                string message = $@"
<html>
<head>
    <link href='https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap' rel='stylesheet'>
    <style>
        body {{
            font-family: 'Roboto', sans-serif;
            background-color: #fff3f3;
            color: #333;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 650px;
            margin: 40px auto;
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 6px 20px rgba(0, 0, 0, 0.1);
            padding: 40px;
        }}
        h1 {{
            color: #d9534f;
            text-align: center;
        }}
        p {{
            font-size: 16px;
            line-height: 1.6;
        }}
        .highlight {{
            font-weight: bold;
            color: #d9534f;
        }}
        .footer {{
            text-align: center;
            font-size: 12px;
            color: #999;
            margin-top: 40px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>Account Deactivated 🔒</h1>
        <p>Dear <strong>{user.FirstName} {user.LastName}</strong>,</p>
        <p>We regret to inform you that your account has been <span class='highlight'>deactivated</span> by the administrator.</p>
        <p>If you believe this action was taken in error or if you need further clarification, please do not hesitate to contact our support team.</p>
        <p>We value your presence and hope to assist you soon.</p>
        <p>Warm regards,<br/>Library Team</p>
        <div class='footer'>
            <p>This is an automated message. Please do not reply directly to this email.</p>
        </div>
    </div>
</body>
</html>";

                await _emailSender.SendEmailAsync(user.Email, subject, message);
            }

            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public async Task<IActionResult> ReactivateUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.IsActive = true;
                _context.SaveChanges();

                string subject = "🎉 Your Account is Reactivated!";
                string message = $@"
<html>
<head>
    <link href='https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap' rel='stylesheet'>
    <style>
        body {{
            font-family: 'Roboto', sans-serif;
            background-color: #e8f0fe;
            color: #333;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 650px;
            margin: 40px auto;
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 6px 20px rgba(0, 0, 0, 0.1);
            padding: 40px;
        }}
        h1 {{
            color: #007bff;
            text-align: center;
        }}
        p {{
            font-size: 16px;
            line-height: 1.6;
        }}
        .button {{
            display: inline-block;
            padding: 12px 24px;
            margin: 20px auto;
            background-color: #007bff;
            color: white;
            text-decoration: none;
            font-weight: bold;
            border-radius: 5px;
        }}
        .footer {{
            text-align: center;
            font-size: 12px;
            color: #999;
            margin-top: 40px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>Welcome Back to the Library! 📚</h1>
        <p>Hello <strong>{user.FirstName} {user.LastName}</strong>,</p>
        <p>We're thrilled to let you know that your account has been <strong>reactivated</strong>! 🎉</p>
        <p>You can now log in and resume enjoying our digital shelves full of amazing books and resources.</p>
        <p style='text-align: center;'>
            <a href='https://your-library-login-url.com' class='button'>Log In Now</a>
        </p>
        <p>If you have any questions, feel free to reach out to our support team anytime.</p>
        <div class='footer'>
            <p>This is an automated message. Please do not reply directly to this email.</p>
        </div>
    </div>
</body>
</html>";

                await _emailSender.SendEmailAsync(user.Email, subject, message);
            }

            return RedirectToAction("ManageUsers");
        }

    }
}

