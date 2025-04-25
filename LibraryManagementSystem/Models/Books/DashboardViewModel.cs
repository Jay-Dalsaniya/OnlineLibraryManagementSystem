//namespace LibraryManagementSystem.Models.Books
//{
//    public class DashboardViewModel
//    {
//        public int TotalBooks { get; set; }
//        public int IssuedBooks { get; set; }
//        public int ReturnedBooks { get; set; }
//        public int AvailableBooks { get; set; }
//        public int TotalAuthors { get; set; }
//        public decimal Penalties { get; set; }
//        public decimal TotalSales { get; set; }
//        public int TotalBooksSold { get; set; }
//        public int TotalIssuedBooks { get; set; }
//        public int TotalReturnedBooks { get; set; }
//        public int TotalUsers { get; internal set; }
//        public object TotalPenalties { get; internal set; }
//        public int TotalBookPurchases { get; internal set; }
//    }
//}


namespace LibraryManagementSystem.Models.Books
{
    public class DashboardViewModel
    {
        // Basic information
        public int TotalBooks { get; set; }
        public int IssuedBooks { get; set; }
        public int ReturnedBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int TotalAuthors { get; set; }
        public int TotalUsers { get; set; }
        public decimal Penalties { get; set; }

        // Sales and transactions
        public decimal TotalBooksSold { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalBookPurchases { get; set; }

        // Additional information
        public int ActiveTransactions { get; set; } // Active transactions (e.g., books in current rental)
        public int OverdueBooks { get; set; } // Overdue books
        public int TotalReturnedBooks { get; set; } // Books that have been returned
        public decimal TotalLateFees { get; set; } // Total late fees incurred
        public int TotalBooksInStock { get; set; } // Total books available in stock (including purchased)
        public int TotalIssuedBooks { get; internal set; }
    }
}