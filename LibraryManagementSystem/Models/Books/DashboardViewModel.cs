namespace LibraryManagementSystem.Models.Books
{
    public class DashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int IssuedBooks { get; set; }
        public int ReturnedBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int TotalAuthors { get; set; }
        public int TotalUsers { get; set; }
        public decimal Penalties { get; set; }

        public decimal TotalBooksSold { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalBookPurchases { get; set; }
        public int ActiveTransactions { get; set; }
        public int OverdueBooks { get; set; } 
        public int TotalReturnedBooks { get; set; } 
        public decimal TotalLateFees { get; set; } 
        public int TotalBooksInStock { get; set; } 
        public int TotalIssuedBooks { get; internal set; }
    }
}