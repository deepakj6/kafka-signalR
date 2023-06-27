using CartServer.Configuration;
using CartServer.Contracts.Requests;
using CartServer.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CartServer.Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var carts = await _cartService.GetAllCarts();
            return Ok(carts);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CartRequest cart)
        {
            if (cart == null)
            {
                return BadRequest();
            }
            var created = await _cartService.CreateCart(cart);
            return Ok(created);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> AddToCart([FromQuery] long id, [FromBody] CartItemRequest cartItem)
        {
            try
            {
                var added = await _cartService.UpdateCart(id, cartItem);
                return Ok(added);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }

}