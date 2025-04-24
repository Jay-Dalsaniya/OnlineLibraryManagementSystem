namespace LibraryManagementSystem.Models.Role
{
    public class ReaderDetailViewModel
    {
        public User Reader { get; set; }
        public List<BookRental> IssuedBooks { get; set; }
        public List<BookRental> ReturnedBooks { get; set; }
        public List<BookPurchase> PurchasedBooks { get; set; }
    }

}
