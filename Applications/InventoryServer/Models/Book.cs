using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace InventoryServer.Models
{
    public class Book
    {
        [Key]        
        public string Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public double Price { get; set; }

        public int Quantity { get; set; }

        public string BookCollectionId { get; set; }
    }
}
