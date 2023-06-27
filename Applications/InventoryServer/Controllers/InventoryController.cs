using InventoryServer.Models;
using InventoryServer.Repository;
using Microsoft.AspNetCore.Mvc;

namespace InventoryServer.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly IInventoryRepository _repository;

        public InventoryController(ILogger<InventoryController> logger, IInventoryRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // Existing GET endpoint to retrieve the inventory
        [HttpGet]
        public ActionResult<Inventory> Get()
        {
            var inventory = _repository.GetInventory();
            return inventory;
        }

        // New POST endpoint to add a book
        [HttpPost("books")]
        public ActionResult<Book> AddBook([FromBody] Book book)
        {
            if (book is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Add the book to the inventory
            _repository.AddBook(book);

            // Return the created book with a 201 Created status
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // New GET endpoint to retrieve a specific book
        [HttpGet("books/{id}")]
        public ActionResult<Book> GetBook(string id)
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
        public ActionResult<Book> UpdateBook(string id, [FromBody] Book updatedBook)
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

            // Return the updated book
            return book;
        }

        // New DELETE endpoint to delete a book
        [HttpDelete("books/{id}")]
        public ActionResult DeleteBook(string id)
        {
            var book = _repository.GetBook(id);

            if (book == null)
            {
                return NotFound();
            }

            // Remove the book from the inventory
            _repository.DeleteBook(id);

            // Return a 204 No Content response
            return NoContent();
        }
    }


}