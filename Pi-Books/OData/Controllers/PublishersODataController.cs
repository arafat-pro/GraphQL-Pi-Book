using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Pi_Books.Data;

namespace Pi_Books.OData.Controllers
{
    public class PublishersODataController:ControllerBase
    {
        private readonly AppDbContext appDbContext;
        public PublishersODataController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(appDbContext.Publishers.AsQueryable());
        }
    }
}