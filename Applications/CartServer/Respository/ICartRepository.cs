using CartServer.Models;

namespace CartServer.Respository
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByIdAsync(long id);
        Task<List<Cart>> GetAllCartsAsync();
        Task<Cart> AddCartAsync(Cart cart);
        Task<Cart> UpdateCartAsync(long id, Cart cart);
        Task DeleteCartAsync(long id);

        Task<CartItem> GetCartItemAsync(long id);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem> UpdateCartItemAsync(long id, CartItem cartItem);
        Task DeleteCartItemAsync(long id);
    }
}
