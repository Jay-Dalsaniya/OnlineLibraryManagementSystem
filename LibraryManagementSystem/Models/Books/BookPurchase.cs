using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class BookPurchase
    {
        public int BookPurchaseId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Author { get; set; }

        [StringLength(13)]
        public string ISBN { get; set; }

        [StringLength(50)]
        public string Genre { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string Subject { get; set; }

        [StringLength(500)]
        public string Summary { get; set; }

        public int TotalCopies { get; set; }

        [StringLength(50)]
        public string Language { get; set; }

        [StringLength(50)]
        public string Edition { get; set; }

        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }


}
