using Microsoft.EntityFrameworkCore;
using Pi_Books.Data;
using Pi_Books.Data.Services;

namespace Pi_Books.Tests.Unit
{
    public partial class PublishersServiceTest
    {
        private static DbContextOptions<AppDbContext> dbContextOptions =>
            new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbTest")
            .Options;

        private static AppDbContext appDbContext;
        PublishersService publishersService;

        [OneTimeSetUp]
        public void Setup()
        {
            appDbContext = new AppDbContext(dbContextOptions);
            appDbContext.Database.EnsureCreated();

            SeedDatabase();

            publishersService = new PublishersService(appDbContext);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            appDbContext.Database.EnsureDeleted();
        }
    }
}