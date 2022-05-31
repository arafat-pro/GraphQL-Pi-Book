using Microsoft.AspNetCore.Mvc;

namespace Pi_Books.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("get-data")]
        public IActionResult Get() => Ok("V2: Sorry Mario, but our princess is in another castle.");
    }
}