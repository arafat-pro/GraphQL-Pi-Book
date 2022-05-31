namespace Pi_Books.Tests.Unit
{
    public partial class PublishersServiceTest
    {
        [Test, Order(1)]
        public void GetAllPublishersWithoutSortingWithoutFilteringWithoutPaging()
        {
            var result = publishersService.GetAllPublishers("", "", null);

            Assert.That(result.Count, Is.EqualTo(4));
            //Assert.AreEqual(3, result.Count);
        }

        [Test, Order(3)]
        public void GetAllPublishersWithoutSortingWithoutFilteringWithPaging()
        {
            var result = publishersService.GetAllPublishers("", "", 2);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test, Order(4)]
        public void GetAllPublishersWithoutSortingWithFilteringWithoutPaging()
        {
            var result = publishersService.GetAllPublishers("", "Lib", null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Manuals Lib"));
        }

        [Test, Order(2)]
        public void GetAllPublishersWithSortingWithoutFilteringWithoutPaging()
        {
            var result = publishersService.GetAllPublishers("name_desc", "", null);
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Pothik Prokashon"));
        }
    }
}