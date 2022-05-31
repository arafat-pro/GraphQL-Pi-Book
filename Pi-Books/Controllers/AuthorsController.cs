using Microsoft.AspNetCore.Mvc;
using Pi_Books.Data.Services;
using Pi_Books.Data.ViewModels;

namespace Pi_Books.Controllers
{
    //Authorization Disabled Temporarily to speed up follow along with the reference lesson
    //[Authorize(Roles =UserRoles.Author)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private AuthorsService authorsService;

        public AuthorsController(AuthorsService authorsService)
        {
            this.authorsService = authorsService;
        }

        [HttpPost("add-author")]
        public IActionResult AddAuthor([FromBody] AuthorVM author)
        {
            authorsService.AddAuthor(author);
            return Ok();
        }

        [HttpGet("single-all-books/{id}")]
        public IActionResult GetTheBook(int id)
        {
            var bookTitles = authorsService.GetAllBooksofAuthor(id);
            return Ok(bookTitles);
        }
    }
}