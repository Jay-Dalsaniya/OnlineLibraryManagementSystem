using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http; 

namespace LibraryManagementSystem.Models
{
    public class BookRental
    {
        public int BookRentalId { get; set; }

        public int UserId { get; set; }

        public int BookId { get; set; }

        public DateTime RentDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal? LateFee { get; set; }

        [StringLength(20)]
        public string RentalStatus { get; set; } 

        [StringLength(50)]
        public string BookCondition { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(100)]
        public string? ModifiedBy { get; set; }

        public virtual User User { get; set; }
        public virtual Book Book { get; set; }

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


        [NotMapped]
        public IFormFile BookImage { get; set; }

        public string Genre { get; set; }  
        public string Category { get; set; }  
        public string Subject { get; set; }  
        public string Summary { get; set; }  

        public int TotalCopies { get; set; }  
        public string Language { get; set; }  
        public string Edition { get; set; }  
        //public decimal RentalFee { get; set; }
       // public decimal? Fine { get; set; }
        public int ReaderId { get; set; } 

    }
}
