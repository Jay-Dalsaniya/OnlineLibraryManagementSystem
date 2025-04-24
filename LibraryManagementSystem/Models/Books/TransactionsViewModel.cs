using X.PagedList;

namespace LibraryManagementSystem.Models.Books
{
    public class TransactionsViewModel
    {

            public List<BookRental> AllRentals { get; set; }
            public List<BookPurchase> AllPurchases { get; set; }

            public decimal TotalLateFees { get; set; }
            public decimal TotalPurchaseAmount { get; set; }

    }

}
