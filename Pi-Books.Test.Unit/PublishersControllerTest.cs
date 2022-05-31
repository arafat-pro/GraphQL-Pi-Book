using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Pi_Books.Controllers;
using Pi_Books.Data;
using Pi_Books.Data.Services;

namespace Pi_Books.Tests.Unit
{
    public partial class PublishersControllerTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions =>
            new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbControllerTest")
            .Options;

        private static AppDbContext appDbContext;
        PublishersService publishersService;
        PublishersController publishersController;

        [OneTimeSetUp]
        public void Setup()
        {
            appDbContext = new AppDbContext(dbContextOptions);
            appDbContext.Database.EnsureCreated();

            SeedDatabase();

            publishersService = new PublishersService(appDbContext);
            publishersController = new PublishersController(publishersService, new NullLogger<PublishersController>());
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            appDbContext.Database.EnsureDeleted();
        }
    }
}