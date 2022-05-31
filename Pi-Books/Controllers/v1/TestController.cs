using Microsoft.AspNetCore.Mvc;

namespace Pi_Books.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiVersion("1.3")]
    [ApiVersion("1.8")]
    [Route("api/[controller]")]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("get-data")]
        public IActionResult Get() => Ok("V1: Sorry Mario, but our princess is in another castle.");

        [HttpGet("get-data"), MapToApiVersion("1.3")]
        public IActionResult GetV13() => Ok("V1.3: Sorry Mario, but our princess is in another castle.");

        [HttpGet("get-data"), MapToApiVersion("1.8")]
        public IActionResult GetV18() => Ok("V1.8: Sorry Mario, but our princess is in another castle.");
    }
}