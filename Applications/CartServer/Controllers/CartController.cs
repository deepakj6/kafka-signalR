using CartServer.Extensions;
using CartServer.Models;
using CartServer.Services.Publishers;
using Microsoft.AspNetCore.Mvc;

namespace CartServer.Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class CartController : ControllerBase
    {
        private readonly ICartEventPublisher _eventPublisher;

        public CartController(ICartEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }
        [HttpGet]
        public IActionResult GetCart()
        {
            // Retrieve the cart from the session
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");

            // If the cart doesn't exist in the session, create a new one
            if (cart == null)
            {
                cart = new Cart();
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return Ok(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(CartItem cartItem)
        {
            // Retrieve the cart from the session
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");

            // If the cart doesn't exist in the session, create a new one
            if (cart == null)
            {
                // Publish the add_to_cart_started event
                _eventPublisher.PublishAddToCartStarted(cartItem.BookId, cartItem.BookTitle, cartItem.BookCollectionId, cartItem.Quantity, cartItem.Price);

                cart = new Cart();
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            // Add the cart item to the cart
            cart.Items.Add(cartItem);

            // Save the updated cart back to the session
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return Ok();
        }

    }

}
