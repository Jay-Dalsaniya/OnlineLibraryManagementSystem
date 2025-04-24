using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var role = User.FindFirst("Role")?.Value;
            Console.WriteLine($"User role: {role}");

            var books = _context.Books
                .Include(b => b.BookRentals) 
                .Include(b => b.BookPurchases) 
                .ToList();

            var bookDetails = books.Select(b => new AdminBookDetailViewModel
            {
                BookId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                Publisher = b.Publisher,
                ISBN = b.ISBN,
                Category = b.Category,
                TotalCopies = b.TotalCopies,
                IssuedCount = b.BookRentals.Count(r => !r.ReturnDate.HasValue), 
                ReturnedCount = b.BookRentals.Count(r => r.ReturnDate.HasValue), 
                PurchasedCount = b.BookPurchases.Count(),
                AvailableCount = b.TotalCopies - b.BookRentals.Count(r => !r.ReturnDate.HasValue)
            }).ToList();

            ViewBag.BookDetails = bookDetails;

            return View();
        }


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
        public IActionResult DeactivateUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.IsActive = false;
                _context.SaveChanges();
            }
            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public IActionResult ReactivateUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.IsActive = true;
                _context.SaveChanges();
            }
            return RedirectToAction("ManageUsers");
        }

    }
}
    
