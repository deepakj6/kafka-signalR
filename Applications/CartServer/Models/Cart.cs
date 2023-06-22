using System.ComponentModel.DataAnnotations;

namespace CartServer.Models
{

    public class Cart
    {
        [Key]
        public string  Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; }
        public decimal TotalPrice { get; set; }

        public decimal Discount { get; set; } = 0;
        
    }
}
