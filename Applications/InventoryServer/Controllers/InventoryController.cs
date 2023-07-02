using InventoryServer.Communication;
using InventoryServer.Models;
using InventoryServer.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace InventoryServer.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly IInventoryRepository _repository;
        private readonly IHubContext<InventoryHub> _inventoryHubContext;

        public InventoryController(ILogger<InventoryController> logger, IInventoryRepository repository, IHubContext<InventoryHub> inventoryHubContext)
        {
            _logger = logger;
            _repository = repository;
            _inventoryHubContext = inventoryHubContext;
        }

        // Existing GET endpoint to retrieve the inventory
        [HttpGet]
        public async Task<ActionResult<Inventory>> Get()
        {
            var inventory = _repository.GetInventory();
            return inventory;
        }

        // New POST endpoint to add a book
        [HttpPost("books")]
        public async Task<ActionResult<Book>> AddBook([FromBody] Book book)
        {
            if (book is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Add the book to the inventory
            _repository.AddBook(book);

            // Send the updated inventory to all connected clients
            await _inventoryHubContext.Clients.All.SendAsync("BookAdded", book);

            // Return the created book with a 201 Created status
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // New GET endpoint to retrieve a specific book
        [HttpGet("books/{id}")]
        public async Task<ActionResult<Book>> GetBook(string id)
        {
            var book = _repository.GetBook(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // New PUT endpoint to update a book
        [HttpPut("books/{id}")]
        public async Task<ActionResult<Book>> UpdateBook(string id, [FromBody] Book updatedBook)
        {
            var book = _repository.GetBook(id);

            if (book == null)
            {
                return NotFound();
            }

            // Update the properties of the existing book
            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Description = updatedBook.Description;
            book.Price = updatedBook.Price;
            book.Quantity = updatedBook.Quantity;

            // Save the changes
            _repository.UpdateBook(book);

            // Send the updated inventory to all connected clients
            await _inventoryHubContext.Clients.All.SendAsync("BookUpdated", book);

            // Return the updated book
            return book;
        }

        // New DELETE endpoint to delete a book
        [HttpDelete("books/{id}")]
        public async Task<ActionResult> DeleteBook(string id)
        {
            var book = _repository.GetBook(id);

            if (book == null)
            {
                return NotFound();
            }

            // Remove the book from the inventory
            _repository.DeleteBook(id);

            // Send the updated inventory to all connected clients
            await _inventoryHubContext.Clients.All.SendAsync("BookDeleted", book);

            // Return a 204 No Content response
            return NoContent();
        }

    }
}