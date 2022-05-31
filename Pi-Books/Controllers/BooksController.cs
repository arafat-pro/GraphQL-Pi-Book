using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi_Books.Data.Services;
using Pi_Books.Data.ViewModels;
using Pi_Books.Data.ViewModels.Authentication;

namespace Pi_Books.Controllers
{
    //Authorization Disabled Temporarily to speed up follow along with the reference lesson
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public BooksService booksService;
        public BooksController(BooksService booksService)
        {
            this.booksService = booksService;
        }

        [HttpGet("all")]
        public IActionResult GetAllBooks()
        {
            var allBooks = booksService.GetAllBooks();
            return Ok(allBooks);
        }

        [HttpGet("single/{id}")]
        public IActionResult GetTheBook(int id)
        {
            var book = booksService.GetTheBook(id);
            return Ok(book);
        }

        //[Authorize(Roles =UserRoles.Author)]
        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] BookVM book)
        {
            booksService.AddBook(book);
            return Ok();
        }

        [HttpPost("add-book-with-authors")]
        public IActionResult AddBookWithAuthors([FromBody] BookVM book)
        {
            booksService.AddBookWithAuthors(book);
            return Ok();
        }

        //[Authorize(Roles = UserRoles.Admin)]
        [HttpPut("modify-one/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] BookVM book)
        {
            var updatedBook = booksService.ModifyBook(id, book);
            return Ok(updatedBook);
        }

        //[Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("delete-one/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            booksService.DeleteTheBook(id);
            return Ok();
        }
    }
}
