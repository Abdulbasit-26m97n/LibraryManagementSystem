using Core;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(IBookService bookService) : ControllerBase
    {
        [HttpGet("{isbn}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<Book>> GetBookByISBNAsync([FromRoute] string isbn)
        {
            var book = await bookService.GetBookByISBNAsync(isbn);
            if (!book.IsSuccess)
            {
                if (book.ErrorType == ResultErrorType.DatabaseError || book.ErrorType == ResultErrorType.UnknownError)
                    return StatusCode(500, new { Message = book.Message });
                return NotFound(new { Message = book.Message });
            }
            return Ok(book.Data);
        }

        [HttpGet("Search")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<ActionResult<Book>> SearchBookAsync([FromQuery] string query)
        {
            var book = await bookService.SearchBookAsync(query.Trim());
            if (!book.IsSuccess)
            {
                if (book.ErrorType == ResultErrorType.DatabaseError || book.ErrorType == ResultErrorType.UnknownError)
                    return StatusCode(500, new { Message = book.Message });
                return NotFound(new { Message = book.Message });
            }
            return Ok(book.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<ActionResult<Book>> AddBookAsync([FromBody] AddBookDto book)
        {
            if (book == null)
            {
                return BadRequest("Book should not be null.");
            }
            else if (string.IsNullOrWhiteSpace(book.ISBN))
            {
                return BadRequest("Book ISBN is required");
            }
            var bookNew = await bookService.AddBookAsync(book);
            if (!bookNew.IsSuccess)
            {
                if (bookNew.ErrorType == ResultErrorType.DatabaseError || bookNew.ErrorType == ResultErrorType.UnknownError)
                    return StatusCode(500, new { Message = bookNew.Message });
                if(bookNew.ErrorType == ResultErrorType.AlreadyExists)
                    return Conflict(new { Message = bookNew.Message });
                return BadRequest(new { Message = bookNew.Message });
            }
            return bookNew.Data!;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<ActionResult<Book>> UpdateBookAsync([FromRoute]int id, [FromBody]Book book)
        {
            var bookUpdated = await bookService.UpdateBookAsync(id, book);
            if (!bookUpdated.IsSuccess)
            {
                if (bookUpdated.ErrorType == ResultErrorType.DatabaseError || bookUpdated.ErrorType == ResultErrorType.UnknownError)
                    return StatusCode(500, new { Message = bookUpdated.Message });
                return BadRequest(new { Message = bookUpdated.Message });
            }
            
            return bookUpdated.Data!;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<ActionResult<BookDto>> DeleteBookAsync([FromRoute]int id)
        {
            var bookDeleted = await bookService.DeleteBookAsync(id);
            if (!bookDeleted.IsSuccess)
            {
                if (bookDeleted.ErrorType == ResultErrorType.DatabaseError || bookDeleted.ErrorType == ResultErrorType.UnknownError)
                    return StatusCode(500, new { Message = bookDeleted.Message });
                return BadRequest(new { Message = bookDeleted.Message });
            }
            return bookDeleted.Data!;
        }
    }
}
