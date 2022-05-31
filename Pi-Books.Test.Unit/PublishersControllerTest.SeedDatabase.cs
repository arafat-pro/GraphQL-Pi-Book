using Pi_Books.Data.Models;

namespace Pi_Books.Tests.Unit
{
    public partial class PublishersControllerTest
    {
        private static void SeedDatabase()
        {
            var publishers = new List<Publisher>
            {
                    new Publisher() { Id=1, Name = "Manuals Lib" },
                    new Publisher() { Id=2, Name = "Open World" },
                    new Publisher() { Id=3, Name = "Maktabatul Furqan" },
                    new Publisher() { Id=4, Name = "Pothik Prokashon" },
                    new Publisher() { Id=5, Name = "Adarsha" },
                    new Publisher() { Id=6, Name = "Mujahid Prokashoni" }
            };
            appDbContext.Publishers.AddRange(publishers);
            appDbContext.SaveChanges();
        }
    }
}