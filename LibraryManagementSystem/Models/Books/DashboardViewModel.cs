namespace LibraryManagementSystem.Models.Books
{
    public class DashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int IssuedBooks { get; set; }
        public int ReturnedBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int TotalAuthors { get; set; }
        public decimal Penalties { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalBooksSold { get; set; }
        public int TotalIssuedBooks { get; set; }
        public int TotalReturnedBooks { get; set; }

    }
}
