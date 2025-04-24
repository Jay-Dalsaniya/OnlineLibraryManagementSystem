namespace LibraryManagementSystem.Models.Books
{
    public class DeleteBookViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int TotalCopies { get; set; }  
        public int CopiesToDelete { get; set; }  
    }

}
