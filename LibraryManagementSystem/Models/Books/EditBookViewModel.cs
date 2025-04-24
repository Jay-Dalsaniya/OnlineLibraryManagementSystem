using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models.Books
{
    public class EditBookViewModel
    {
        public int BookId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [StringLength(13)]
        public string ISBN { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyBy { get; set; }

        public DateTime ModifyDate { get; set; }

        public string ImagePath { get; set; }

        [StringLength(100)]
        public string Genre { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        [StringLength(100)]
        public string Subject { get; set; }

        [StringLength(50)]
        public string Condition { get; set; }

        [StringLength(1000)]
        public string Summary { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1.")]
        public int TotalCopies { get; set; }

        [StringLength(50)]
        public string Language { get; set; }

        [StringLength(50)]
        public string Edition { get; set; }
    }
}
