using AutoMapper;
using CartServer.Configuration;
using CartServer.Contracts.Requests;
using CartServer.Contracts.Responses;
using CartServer.Models;
using CartServer.Respository;
using CartServer.Services.Contracts;
using CartServer.Services.Publishers;


namespace CartServer.Services
{
    public class CartService : ICartService
    {
        private readonly ICartEventPublisher _eventPublisher;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartEventPublisher eventPublisher, ICartRepository cartRepository, IMapper mapper)
        {
            _eventPublisher = eventPublisher;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartResponse> CreateCart(CartRequest cartRequest)
        {
            // Publish the add_to_cart_started event
            // _eventPublisher.PublishAddToCartStarted(cartItem.BookId, cartItem.BookTitle, cartItem.BookCollectionId, cartItem.Quantity, cartItem.Price);
            var cart = _mapper.Map<Cart>(cartRequest);
            var addedCart = await _cartRepository.AddCartAsync(cart);
            return _mapper.Map<CartResponse>(addedCart);
        }

        public async Task DeleteCart(long id)
        {
            await _cartRepository.DeleteCartAsync(id);
        }

        public async Task DeleteCartItem(long id)
        {
            await _cartRepository.DeleteCartItemAsync(id);
        }

        public async Task<IEnumerable<CartResponse>> GetAllCarts()
        {
            var carts = await _cartRepository.GetAllCartsAsync();
            var cartResponses = _mapper.Map<IEnumerable<CartResponse>>(carts);
            return cartResponses;
        }

        public async Task<CartResponse> GetCartById(long id)
        {
            var cart = await _cartRepository.GetCartByIdAsync(id);
            var cartResponse = _mapper.Map<CartResponse>(cart);
            return cartResponse;
        }

        public async Task<CartResponse> UpdateCart(long id, CartRequest cartRequest)
        {
            var existingCart = await _cartRepository.GetCartByIdAsync(id);
            if (existingCart == null)
                throw new NotFoundException("Cart not found.");

            var updatedCart = _mapper.Map(cartRequest, existingCart);
            var cart = await _cartRepository.UpdateCartAsync(id, updatedCart);
            var cartResponse = _mapper.Map<CartResponse>(cart);
            return cartResponse;
        }

        public async Task<CartResponse> UpdateCart(long cartId, CartItemRequest cartItemRequest)
        {
            var cart = await _cartRepository.GetCartByIdAsync(cartId);
            if (cart == null)
                throw new NotFoundException("Cart not found.");

            var cartItem = _mapper.Map<CartItem>(cartItemRequest);
            cart.Items.Add(cartItem);
            cart.TotalPrice += cartItem.Price;

            var updatedCart = await _cartRepository.UpdateCartAsync(cartId, cart);
            var cartResponse = _mapper.Map<CartResponse>(updatedCart);
            return cartResponse;
        }
    }

}
