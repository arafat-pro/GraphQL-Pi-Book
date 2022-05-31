using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pi_Books.Data.Services;
using Pi_Books.Data.ViewModels;
using Pi_Books.Exceptions;

namespace Pi_Books.Controllers
{
    //Authorization Disabled Temporarily to speed up follow along with the reference lesson
    //[Authorize(Roles = UserRoles.Admin+","+UserRoles.Publisher)]
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private PublishersService publishersService;
        private readonly ILogger<PublishersController> logger;

        public PublishersController(PublishersService publishersService, ILogger<PublishersController> logger)
        {
            this.publishersService = publishersService;
            this.logger = logger;
        }

        [HttpGet("all")]
        public IActionResult GetAllPublishers(string sortBy, string searchText, int pageNumber)
        {
            //throw new Exception("Arbitrary Exception thrown from Default Controller Endpoint to Logging Exception Test!");
            try
            {
                logger.LogInformation("Very 1st log in Get all Publishers.");
                var publishers = publishersService.GetAllPublishers(sortBy, searchText, pageNumber);
                return Ok(publishers);
            }
            catch (Exception)
            {
                return BadRequest("Publishers couldn't be Loaded!");
            }
        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher)
        {
            try
            {
                var newPublisher = publishersService.AddPublisher(publisher);

                return Created(nameof(AddPublisher), newPublisher);
            }
            catch (PublisherNameException exception)
            {
                return BadRequest($"{exception.Message}, Publisher Name: {exception.PublisherName}");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("single/{id}")]
        public IActionResult GetThePublisher(int id)
        {
            //throw new Exception("This is an exception that will be handled by middle-ware!");

            var publisher = publishersService.GetThePublisher(id);

            if (publisher != null)
            {
                //var responseObject = new CustomActionResultVM()
                //{
                //    Publisher = publisher
                //};

                //return new CustomActionResult(responseObject);
                //return publisher;
                return Ok(publisher);
            }
            else
            {
                return NotFound();
                //var responseObject = new CustomActionResultVM()
                //{
                //    Exception = new Exception("This is coming form the Publisher Controller Custom Action Result Exception")
                //};
                //return new CustomActionResult(responseObject);
            }
        }

        [HttpGet("single-all-books-with-authors/{id}")]
        public IActionResult GetAllBookAndAuthors(int id)
        {
            var _bookAndAuthors = publishersService.GetAllBooksAndAuthorsOfPublisher(id);
            return Ok(_bookAndAuthors);
        }

        [HttpDelete("delete-one/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            try
            {
                publishersService.DeleteThePublisher(id);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}