namespace LibraryManagementSystem.Models.Role
{
    public class AdminBookDetailViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int TotalCopies { get; set; }
        public int IssuedCount { get; set; }
        public int ReturnedCount { get; set; }
        public int PurchasedCount { get; set; }
        public int AvailableCount { get; set; }

        public string Author { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public string Category { get; set; }
    }
}
