using Library.Data.Interfaces.Services;
using Library.Data.Models.Requests;
using Library.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService) 
        {
            _bookService = bookService;
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetAllBooks()
        {
            var categories = _bookService.GetAllBooks();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetbookById(int id)
        {
            var result = await _bookService.GetBookByIdAsync(id);
            if (!result.Success)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateBook(BookModel book)
        {
            var result = await _bookService.CreateBookAsync(book);
            return Ok(result);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateBook(int id, BookModel book)
        {
            var result = await _bookService.UpdateBookAsync(id, book);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            return Ok(result);
        }
    }
}
