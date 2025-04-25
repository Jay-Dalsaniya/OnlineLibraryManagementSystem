using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagementSystem.Models.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LibraryManagementSystem.Models.Role;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.SignalR;
using DocumentFormat.OpenXml.Bibliography;

namespace LibraryManagementSystem.Controllers
{
    public class LibrarianController : Controller
    {
        private readonly ApplicationDbContext _context;

      
        public LibrarianController(ApplicationDbContext context)
        {
            _context = context;
         
        }


      
        private decimal CalculateLateFee(BookRental rental)
        {
            if (rental.ReturnDate.HasValue && rental.ReturnDate > rental.DueDate)
            {
                var lateDays = (rental.ReturnDate.Value - rental.DueDate).Days;
                return lateDays * 1.5m; 
               

            }
            return 0;
        }

        private void UpdateRentalStatus(BookRental rental)
        {
            if (rental.ReturnDate.HasValue)
            {
                rental.RentalStatus = "Returned";
            }
            else if (rental.DueDate < DateTime.Now)
            {
                rental.RentalStatus = "Overdue";
            }
            else
            {
                rental.RentalStatus = "Active";
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRental(int rentalId)
        {
            var rental = await _context.BookRentals.FirstOrDefaultAsync(br => br.BookRentalId == rentalId);

            if (rental == null)
            {
                TempData["ErrorMessage"] = "Rental not found.";
                return RedirectToAction("Index");
            }

            rental.LateFee = CalculateLateFee(rental);

            UpdateRentalStatus(rental);

            rental.ModifiedDate = DateTime.Now;
            rental.ModifiedBy = User.Identity?.Name ?? "System";

            _context.Update(rental);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Rental updated successfully!";
            return RedirectToAction("Index");
        }
        public IActionResult Index(string searchTerm)
        {
            var booksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                booksQuery = booksQuery.Where(b =>
                    b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm) || b.ISBN.Contains(searchTerm));
            }

            var books = booksQuery.ToList();
            ViewData["SearchTerm"] = searchTerm;

            return View(books);
        }

        public IActionResult MyBooks(string searchTerm)
        {
            var userName = User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                TempData["ErrorMessage"] = "Unable to retrieve your books.";
                return RedirectToAction("Index");
            }

            var booksQuery = _context.Books.Where(b => b.CreatedBy == userName);

            // Filter by search term if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                booksQuery = booksQuery.Where(b =>
                    b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm));
            }

            var books = booksQuery.ToList();
            ViewData["SearchTerm"] = searchTerm;

            return View(books);
        }

        public async Task<IActionResult> ActiveRentals()
        {
            var activeRentals = await _context.BookRentals
                .Include(br => br.Book)
                .Include(br => br.User)
                .Where(br => br.RentalStatus == "Active")
                .OrderByDescending(br => br.RentDate)
                .ToListAsync();

            return View(activeRentals);
        }

        public async Task<IActionResult> Dashboard()
        {
            var totalBooks = await _context.Books.SumAsync(b => b.TotalCopies);

            var issuedBooks = await _context.BookRentals.CountAsync(br => br.RentalStatus == "Active");

            var returnedBooks = await _context.BookRentals.CountAsync(br => br.RentalStatus == "Returned");

            var availableBooks = await _context.Books
                .Select(b => b.TotalCopies - _context.BookRentals
                    .Where(br => br.BookId == b.BookId && br.RentalStatus == "Active")
                    .Count())
                .SumAsync();

            var totalAuthors = await _context.Books.Select(b => b.Author).Distinct().CountAsync();

            var penalties = await _context.BookRentals
                .Where(br => br.LateFee.HasValue && br.LateFee > 0)
                .SumAsync(br => br.LateFee.Value);

            var totalSales = await _context.BookPurchases
                .SumAsync(purchase => purchase.Price);

            var totalBooksSold = await _context.BookPurchases.CountAsync();

            var totalIssuedBooks = await _context.BookRentals.CountAsync();

            var totalReturnedBooks = await _context.BookRentals
                .Where(br => br.RentalStatus == "Returned")
                .CountAsync();

            var dashboardData = new DashboardViewModel
            {
                TotalBooks = totalBooks,
                IssuedBooks = issuedBooks,
                ReturnedBooks = returnedBooks,
                AvailableBooks = availableBooks,
                TotalAuthors = totalAuthors,
                Penalties = penalties,
                TotalSales = totalSales,
                TotalBooksSold = totalBooksSold,
                TotalIssuedBooks = totalIssuedBooks,
                TotalReturnedBooks = totalReturnedBooks
            };

            return View(dashboardData);
        }



        public IActionResult AddBook()
        {
            return View(new AddBookViewModel()); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(AddBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imagePath = null;

                if (model.BookImage != null && model.BookImage.Length > 0)
                {
                    var fileName = Path.GetFileName(model.BookImage.FileName);
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/books", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(uploadPath));

                    using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await model.BookImage.CopyToAsync(fileStream);
                    }

                    imagePath = $"/images/books/{fileName}";
                }

                var book = new Book
                {
                    Title = model.Title,
                    Author = model.Author,
                    ISBN = model.ISBN,
                    Price = model.Price,
                    PublishedDate = model.PublishedDate,
                    Category = model.Category,
                    Subject = model.Subject,
                    Condition = model.Condition,
                    Summary = model.Summary,
                    TotalCopies = model.TotalCopies,
                    SellBook = model.SellBook,
                    Genre = model.Genre,
                    AvailableCopies = model.TotalCopies,
                    Language = model.Language,
                    Edition = model.Edition,
                    Publisher = User.Identity?.Name ?? "System",
                    CreatedBy = User.Identity?.Name ?? "System",
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    ModifyBy = User.Identity?.Name ?? "System",
                    ImagePath = imagePath
                };

                _context.Add(book);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Book added successfully!";
                return RedirectToAction("Index", "Librarian");
            }

            TempData["ErrorMessage"] = "There were some errors in the form.";
            return View(model);
        }

        public async Task<IActionResult> EditBook(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found.";
                return RedirectToAction("Index", "Librarian");
            }

            var model = new EditBookViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Price = book.Price,
                PublishedDate = book.PublishedDate,
                CreatedBy = book.CreatedBy,
                CreateDate = book.CreateDate,
                ModifyDate = book.ModifyDate,
                ModifyBy = book.ModifyBy,
                ImagePath = book.ImagePath,
                Genre = book.Genre,
                Category = book.Category,
                Subject = book.Subject,
                Condition = book.Condition,
                Summary = book.Summary,
                TotalCopies = book.TotalCopies,
                SellBook = book.SellBook,
                Language = book.Language,
                Edition = book.Edition
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(EditBookViewModel model, IFormFile BookImage)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == model.BookId);

            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found.";
                return RedirectToAction("Index", "Librarian");
            }

            string imagePath = book.ImagePath;

            if (BookImage != null && BookImage.Length > 0)
            {
                var fileExtension = Path.GetExtension(BookImage.FileName);
                var fileName = Path.GetFileNameWithoutExtension(BookImage.FileName);
                var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";
                var uploadPath = Path.Combine("wwwroot/images/books", uniqueFileName);

                var newImageVirtualPath = $"/images/books/{uniqueFileName}";

                var imageExists = await _context.Books
                    .AnyAsync(b => b.ImagePath == newImageVirtualPath && b.BookId != model.BookId);

                if (imageExists)
                {
                    TempData["ErrorMessage"] = "This image is already in use by another book. Please upload a different image.";
                    return View(model);
                }

                Directory.CreateDirectory(Path.GetDirectoryName(uploadPath));

                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    await BookImage.CopyToAsync(fileStream);
                }

                imagePath = newImageVirtualPath;

                if (!string.IsNullOrEmpty(book.ImagePath))
                {
                    var oldImagePath = Path.Combine("wwwroot", book.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deleting old image: {ex.Message}");
                        }
                    }
                }
            }

            book.Title = model.Title;
            book.Author = model.Author;
            book.ISBN = model.ISBN;
            book.Price = model.Price;
            book.PublishedDate = model.PublishedDate;
            book.ModifyDate = DateTime.Now;
            book.ModifyBy = User.Identity?.Name ?? "System";
            book.ImagePath = imagePath;
            book.Genre = model.Genre;
            book.Category = model.Category;
            book.Subject = model.Subject;
            book.Condition = model.Condition;
            book.Summary = model.Summary;
            book.TotalCopies = model.TotalCopies;
            book.SellBook = model.SellBook;
            book.Language = model.Language;
            book.Edition = model.Edition;

            if (model.TotalCopies != book.TotalCopies)
            {
                book.AvailableCopies = model.TotalCopies;
            }

            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Book updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "There was an error updating the book.";
                Console.WriteLine($"Error: {ex.Message}");
            }

            return RedirectToAction("Index", "Librarian");
        }

        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookRentals) 
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found.";
                return RedirectToAction("Index", "Librarian");
            }

            if (book.BookRentals.Any(br => br.RentalStatus == "Active"))
            {
                TempData["ErrorMessage"] = "This book cannot be deleted because it is currently issued to a reader.";
                return RedirectToAction("Index", "Librarian");
            }

            var model = new DeleteBookViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                TotalCopies = book.TotalCopies
            };

            return View(model);
        }

        [HttpPost, ActionName("DeleteBook")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int copiesToDelete)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found.";
                return RedirectToAction("Index", "Librarian");
            }

            if (copiesToDelete <= 0)
            {
                TempData["ErrorMessage"] = "Please specify a valid number of copies to delete.";
                return RedirectToAction("DeleteBook", new { id = book.BookId });
            }

            if (copiesToDelete > book.TotalCopies)
            {
                TempData["ErrorMessage"] = "Cannot delete more copies than available.";
                return RedirectToAction("DeleteBook", new { id = book.BookId });
            }

            book.TotalCopies -= copiesToDelete;

            book.AvailableCopies = Math.Min(book.AvailableCopies, book.TotalCopies);

            if (book.TotalCopies == 0)
            {
                book.IsDeleted = true;
                TempData["InfoMessage"] = $"All copies of the book '{book.Title}' are now marked as deleted.";
            }
            else
            {
                TempData["SuccessMessage"] = $"{copiesToDelete} copy(s) of the book '{book.Title}' deleted successfully.";
            }

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Librarian");
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllIssuedBooks(string searchTerm = "", int page = 1)
        {
            // Query for active book rentals
            var issuedBooksQuery = _context.BookRentals
                .Include(br => br.Book)
                .Include(br => br.User)
                .AsQueryable();

            // Apply search functionality
            if (!string.IsNullOrEmpty(searchTerm))
            {
                issuedBooksQuery = issuedBooksQuery
                    .Where(br => br.Book.Title.Contains(searchTerm) || br.ISBN.Contains(searchTerm)||
                                 (br.User.FirstName + " " + br.User.LastName).Contains(searchTerm)); // Search by Book Title or Reader's Name
            }

            // Apply pagination
            var issuedBooksList = await issuedBooksQuery
                .OrderBy(br => br.RentDate) // You can change the order if needed
                .ToListAsync();

            var issuedBooksPaged = issuedBooksList.ToPagedList(page, 10); // Page size of 10

            // Pass search term to the view
            ViewData["SearchTerm"] = searchTerm;

            return View(issuedBooksPaged);
        }
        [HttpGet]
        public async Task<IActionResult> ExportIssuedBooksToExcel(string searchTerm = "")
        {
            var issuedBooksQuery = _context.BookRentals
                .Include(br => br.Book)
                .Include(br => br.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                issuedBooksQuery = issuedBooksQuery.Where(br =>
                    br.Book.Title.Contains(searchTerm) ||
                    br.Book.ISBN.Contains(searchTerm) ||
                    (br.User.FirstName + " " + br.User.LastName).Contains(searchTerm));
            }

            var data = await issuedBooksQuery.OrderBy(br => br.RentDate).ToListAsync();

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("IssuedBooks");

            // Header row
            worksheet.Cell(1, 1).Value = "Reader Name";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Book Title";
            worksheet.Cell(1, 4).Value = "ISBN";
            worksheet.Cell(1, 5).Value = "Rent Date";
            worksheet.Cell(1, 6).Value = "Due Date";

            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.User.FirstName + " " + item.User.LastName;
                worksheet.Cell(row, 2).Value = item.User.Email;
                worksheet.Cell(row, 3).Value = item.Book.Title;
                worksheet.Cell(row, 4).Value = item.Book.ISBN;
                worksheet.Cell(row, 5).Value = item.RentDate.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 6).Value = item.DueDate.ToString("yyyy-MM-dd");
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "IssuedBooks.xlsx");
        }


        [HttpGet]
        public async Task<IActionResult> ViewAllReturnedBooks(string searchTerm = "", int page = 1)
        {
            // Query for returned book rentals
            var returnedBooksQuery = _context.BookRentals
                .Where(br => br.RentalStatus == "Returned")
                .Include(br => br.Book)
                .Include(br => br.User)
                .AsQueryable();

            // Apply search functionality
            if (!string.IsNullOrEmpty(searchTerm))
            {
                returnedBooksQuery = returnedBooksQuery
                    .Where(br => br.Book.Title.Contains(searchTerm) || br.Book.ISBN.Contains(searchTerm)||
                                 (br.User.FirstName + " " + br.User.LastName).Contains(searchTerm)); // Search by Book Title or Reader's Name
            }

            // Apply pagination
            var returnedBooksList = await returnedBooksQuery
                .OrderBy(br => br.ReturnDate) // You can change the order if needed
                .ToListAsync();

            var returnedBooksPaged = returnedBooksList.ToPagedList(page, 10); // Page size of 10

            // Pass search term to the view
            ViewData["SearchTerm"] = searchTerm;

            return View(returnedBooksPaged);
        }
        [HttpGet]
        public async Task<IActionResult> ExportReturnedBooksToExcel(string searchTerm = "")
        {
            var returnedBooksQuery = _context.BookRentals
                .Where(br => br.RentalStatus == "Returned")
                .Include(br => br.Book)
                .Include(br => br.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                returnedBooksQuery = returnedBooksQuery
                    .Where(br => br.Book.Title.Contains(searchTerm) ||
                                 br.Book.ISBN.Contains(searchTerm) ||
                                 (br.User.FirstName + " " + br.User.LastName).Contains(searchTerm));
            }

            var returnedBooks = await returnedBooksQuery.OrderBy(br => br.ReturnDate).ToListAsync();

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("ReturnedBooks");

            worksheet.Cell(1, 1).Value = "Reader Name";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Book Title";
            worksheet.Cell(1, 4).Value = "ISBN";
            worksheet.Cell(1, 5).Value = "Rent Date";
            worksheet.Cell(1, 6).Value = "Return Date";

            int row = 2;
            foreach (var br in returnedBooks)
            {
                worksheet.Cell(row, 1).Value = $"{br.User.FirstName} {br.User.LastName}";
                worksheet.Cell(row, 2).Value = br.User.Email;
                worksheet.Cell(row, 3).Value = br.Book.Title;
                worksheet.Cell(row, 4).Value = br.Book.ISBN;
                worksheet.Cell(row, 5).Value = br.RentDate.ToShortDateString();
                worksheet.Cell(row, 6).Value = br.ReturnDate?.ToShortDateString();
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReturnedBooks.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllPurchasedBooks(string searchTerm = "", int page = 1)
        {
            // Query for purchased books
            var purchasedBooksQuery = _context.BookPurchases
                .Include(bp => bp.Book)
                .Include(bp => bp.User)
                .AsQueryable();

            // Apply search functionality
            if (!string.IsNullOrEmpty(searchTerm))
            {
                purchasedBooksQuery = purchasedBooksQuery
                    .Where(bp => bp.Book.Title.Contains(searchTerm) ||
                                 bp.Book.ISBN.Contains(searchTerm) ||
                                 (bp.User.FirstName + " " + bp.User.LastName).Contains(searchTerm)); // Search by Book Title or Reader's Name
            }

            // Apply pagination
            var purchasedBooksList = await purchasedBooksQuery
                .OrderBy(bp => bp.PurchaseDate) // Sorting by purchase date, adjust as necessary
                .ToListAsync();

            var purchasedBooksPaged = purchasedBooksList.ToPagedList(page, 10); // Page size of 10

            // Pass search term to the view
            ViewData["SearchTerm"] = searchTerm;

            return View(purchasedBooksPaged);
        }
        [HttpGet]
        public async Task<IActionResult> ExportPurchasedBooksToExcel(string searchTerm = "")
        {
            var purchasedBooksQuery = _context.BookPurchases
                .Include(bp => bp.Book)
                .Include(bp => bp.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                purchasedBooksQuery = purchasedBooksQuery
                    .Where(bp => bp.Book.Title.Contains(searchTerm) ||
                                 bp.Book.ISBN.Contains(searchTerm) ||
                                 (bp.User.FirstName + " " + bp.User.LastName).Contains(searchTerm));
            }

            var purchases = await purchasedBooksQuery.OrderBy(bp => bp.PurchaseDate).ToListAsync();

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("PurchasedBooks");

            worksheet.Cell(1, 1).Value = "Reader Name";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Book Title";
            worksheet.Cell(1, 4).Value = "ISBN";
            worksheet.Cell(1, 5).Value = "Price";
            worksheet.Cell(1, 6).Value = "Purchase Date";

            int row = 2;
            foreach (var bp in purchases)
            {
                worksheet.Cell(row, 1).Value = $"{bp.User.FirstName} {bp.User.LastName}";
                worksheet.Cell(row, 2).Value = bp.User.Email;
                worksheet.Cell(row, 3).Value = bp.Book.Title;
                worksheet.Cell(row, 4).Value = bp.Book.ISBN;
                worksheet.Cell(row, 5).Value = bp.Price;
                worksheet.Cell(row, 6).Value = bp.PurchaseDate.ToShortDateString();
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PurchasedBooks.xlsx");
        }


        [HttpGet]
        public IActionResult ViewReaderDetails(int userId)
        {
            var reader = _context.Users
                .Include(u => u.BookRentals)
                    .ThenInclude(br => br.Book)
                .Include(u => u.BookPurchases)
                    .ThenInclude(bp => bp.Book)
                .FirstOrDefault(u => u.Id == userId);

            if (reader == null)
            {
                TempData["ErrorMessage"] = "Reader not found.";
                return RedirectToAction("ViewAllPurchasedBooks");
            }

            return View(reader);
        }

        public IActionResult AllTransactions()
        {
            var allRentals = _context.BookRentals
                .Include(r => r.Book)
                .Include(r => r.User)
                .Where(r => r.LateFee >0)
                .OrderByDescending(r => r.RentDate)
                .ToList();

            var allPurchases = _context.BookPurchases
                .Include(p => p.Book)
                .Include(p => p.User)
                .OrderByDescending(p => p.PurchaseDate)
                .ToList();

            var totalLateFees = allRentals.Sum(r => r.LateFee ?? 0);
            var totalPurchaseAmount = allPurchases.Sum(p => p.Price);

            var viewModel = new TransactionsViewModel
            {
                AllRentals = allRentals,
                AllPurchases = allPurchases,
                TotalLateFees = totalLateFees,
                TotalPurchaseAmount = totalPurchaseAmount
            };

            return View(viewModel);
        }

        public IActionResult BookDetails(int id, string? source)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Book not found!";
                return RedirectToAction("MyBooks"); 
            }
            ViewBag.Source = source; 

            return View(book);
        }

    }
}
