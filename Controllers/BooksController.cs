using Microsoft.AspNetCore.Mvc;
using pr11.Models;
using pr11.Services;

namespace pr11.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _service;

        public BooksController(BookService service)
        {
            _service = service;
        }

        // GET: api/books
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _service.GetAllAsync();
            return Ok(books);
        }

        // GET: api/books/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _service.GetByIdAsync(id);

            if (book == null)
                return NotFound(new { message = "Book not found" });

            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                return BadRequest(new { message = "Title is required" });

            var newBook = await _service.AddAsync(book);

            return CreatedAtAction(nameof(GetById),
                new { id = newBook.Id }, newBook);
        }
    }
}