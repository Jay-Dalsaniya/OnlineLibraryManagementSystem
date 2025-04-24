using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; 

namespace LibraryManagementSystem.Models.Books
{
    public class AddBookViewModel
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [StringLength(13)]  // ISBN
        public string ISBN { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }

        [Display(Name = "Book Cover Image")]
        public IFormFile BookImage { get; set; }

        public string Genre { get; set; }  
        public string Category { get; set; } 
        public string Subject { get; set; }  
        public string Condition { get; set; }  
        public string Summary { get; set; }  

        [Required]
        public int TotalCopies { get; set; }  
        public string Language { get; set; }  
        public string Edition { get; set; }  

    }
}
