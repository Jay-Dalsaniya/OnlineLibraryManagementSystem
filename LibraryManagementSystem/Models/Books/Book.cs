using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Book
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

        public bool IsAvailable => AvailableCopies > 0;  

        public decimal Price { get; set; }  

        public DateTime PublishedDate { get; set; }  

        public string CreatedBy { get; set; }  

        public DateTime CreateDate { get; set; }  
        public string ModifyBy { get; set; } 

        public DateTime ModifyDate { get; set; }  

        [StringLength(255)]
        public string? ImagePath { get; set; } 

        public string Genre { get; set; }  

        public string Language { get; set; }  

        public int TotalCopies { get; set; }  

        public int AvailableCopies { get; set; }  
        public DateTime DateAdded { get; set; }  

        public bool IsDeleted { get; set; } = false;

        public string Edition { get; set; }  

        public string Publisher { get; set; } 
        [Range(0, 5)]
        public double Rating { get; set; }  

        // New fields
        public string Category { get; set; }  

        public string Subject { get; set; } 

        public string Condition { get; set; }

        public string Summary { get; set; }

        public int SellBook { get; set; } 

        public ICollection<BookRental> BookRentals { get; set; } = new List<BookRental>();
        public ICollection<BookPurchase> BookPurchases { get; set; } = new List<BookPurchase>();

    }
}
