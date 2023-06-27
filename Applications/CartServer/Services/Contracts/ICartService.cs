using CartServer.Contracts.Requests;
using CartServer.Contracts.Responses;

namespace CartServer.Services.Contracts
{
    public interface ICartService
    {
        Task<IEnumerable<CartResponse>> GetAllCarts();
        Task<CartResponse> GetCartById(long id);

        Task<CartResponse> CreateCart(CartRequest cartRequest);

        Task<CartResponse> UpdateCart(long id, CartRequest cartRequest);
        Task<CartResponse> UpdateCart(long cartId, CartItemRequest cartItemRequest);

        Task DeleteCart(long id);

        Task DeleteCartItem(long id);
    }
}
