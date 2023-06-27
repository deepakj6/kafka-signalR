using CartServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CartServer.Respository
{
    public class SqlCartRepository : ICartRepository
    {
        private readonly CartDbContext _dbContext;

        public SqlCartRepository(CartDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Cart> AddCartAsync(Cart cart)
        {
            _ = await _dbContext.Carts.AddAsync(cart);
            _ = await _dbContext.SaveChangesAsync();
            var added = await GetCartByIdAsync(cart.Id);
            return added;
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            _ = _dbContext.CartItems.AddAsync(cartItem);
            _ = _dbContext.SaveChangesAsync();
            return await GetCartItemAsync(cartItem.Id);

        }

        public async Task DeleteCartAsync(long id)
        {
            var found = await GetCartByIdAsync(id);
            if (found is not null)
            {
                _dbContext.Carts.Remove(found);
                _ = _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteCartItemAsync(long id)
        {
            var found = await GetCartItemAsync(id);
            if (found is not null)
            {
                _dbContext.CartItems.Remove(found);
                _ = _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Cart>> GetAllCartsAsync()
        {
            return await _dbContext.Carts.ToListAsync();
        }

        public async Task<Cart> GetCartByIdAsync(long id)
        {
            var found = await _dbContext.Carts.FindAsync(id);
            return found!;
        }

        public async Task<CartItem> GetCartItemAsync(long id)
        {
            var found = await _dbContext.CartItems.FindAsync(id);
            return found!;
        }

        public async Task<Cart> UpdateCartAsync(long id, Cart cart)
        {
            var found = await GetCartByIdAsync(cart.Id);
            if (found != null)
            {
                cart.Id = id;
                found = cart;
                _dbContext.Carts.Update(found);
                _ = _dbContext.SaveChangesAsync();
            }
            return found;
        }

        public async Task<CartItem> UpdateCartItemAsync(long id, CartItem cartItem)
        {
            var found = await GetCartItemAsync(id);
            if (found != null)
            {
                cartItem.Id = id;
                found = cartItem;
                _dbContext.CartItems.Update(found);
                _ = _dbContext.SaveChangesAsync();
            }
            return found;
        }
    }

}
