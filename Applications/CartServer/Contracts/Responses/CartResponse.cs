using CartServer.Models;

namespace CartServer.Contracts.Responses
{
    public class CartResponse
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; } = 0;
    }
}
