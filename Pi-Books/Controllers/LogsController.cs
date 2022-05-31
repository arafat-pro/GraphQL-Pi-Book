using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi_Books.Data.Services;
using Pi_Books.Data.ViewModels.Authentication;

namespace Pi_Books.Controllers
{
    //Authorization Disabled Temporarily to speed up follow along with the reference lesson
    //[Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private LogsService logsService;
        public LogsController(LogsService logsService)
        {
            this.logsService = logsService;
        }

        [HttpGet]
        public ActionResult<string> Get() =>
            Ok("Sorry Mario, but our princess is in another castle.");

        [HttpGet("all-from-db")]
        public IActionResult GetAllFromDatabase()
        {
            try
            {
                var allLogs = logsService.GetAllLogsFromDatabase();
                return Ok(allLogs);
            }
            catch (System.Exception)
            {
                return BadRequest("Loading Logs from Database Failed!");
            }
        }
    }
}