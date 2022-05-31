using Microsoft.AspNetCore.Mvc;
using Pi_Books.Data.Models;
using Pi_Books.Data.ViewModels;

namespace Pi_Books.Tests.Unit
{
    public partial class PublishersControllerTest
    {
        [Test, Order(1)]
        public void HttpGetAllPublishersWithSortingFilteringAndPagingReturnOkTest()
        {
            IActionResult actionResult = publishersController.GetAllPublishers("name_desc", "a", 1);
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var actionResultData = (actionResult as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResultData.Count, Is.EqualTo(4));
            Assert.That(actionResultData.FirstOrDefault().Name, Is.EqualTo("Pothik Prokashon"));
            Assert.That(actionResultData.FirstOrDefault().Id, Is.EqualTo(4));

            IActionResult actionResult2ndPage = publishersController.GetAllPublishers("name_desc", "a", 2);
            Assert.That(actionResult2ndPage, Is.Not.Null);
            Assert.That(actionResult2ndPage, Is.TypeOf<OkObjectResult>());
            var actionResultData2ndPage = (actionResult2ndPage as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResultData2ndPage.Count, Is.EqualTo(1));
            Assert.That(actionResultData2ndPage.FirstOrDefault().Name, Is.EqualTo("Adarsha"));
            Assert.That(actionResultData2ndPage.FirstOrDefault().Id, Is.EqualTo(5));
        }

        [Test, Order(2)]
        public void HttpGetPublisherSingleByIdReturnOkTest()
        {
            int publisherId = 5;
            IActionResult actionResult = publishersController.GetThePublisher(publisherId);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            var publisherData = (actionResult as OkObjectResult).Value as Publisher;
            Assert.That(publisherData.Id, Is.EqualTo(5));
            Assert.That(publisherData.Name, Is.EqualTo("adarsha").IgnoreCase);
        }

        [Test, Order(3)]
        public void HttpGetPublisherSingleByIdReturnNotFoundTest()
        {
            int publisherId = 55;
            IActionResult actionResult = publishersController.GetThePublisher(publisherId);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>());
        }

        [Test, Order(4)]
        public void HttpPostAddPublisherReturnCreatedTest()
        {
            var publisherVM = new PublisherVM()
            {
                Name = "New Publisher"
            };
            IActionResult actionResult = publishersController.AddPublisher(publisherVM);

            Assert.That(actionResult, Is.TypeOf<CreatedResult>());
        }

        [Test, Order(5)]
        public void HttpPostAddPublisherReturnBadRequestTest()
        {
            var publisherVM = new PublisherVM()
            {
                Name = "123 New Publisher"
            };
            IActionResult actionResult = publishersController.AddPublisher(publisherVM);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(6)]
        public void HttpDeletePublisherByIdReturnOkTest()
        {
            int publisherId = 5;
            IActionResult actionResult = publishersController.DeletePublisherById(publisherId);
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(7)]
        public void HttpDeletePublisherByIdReturnBadRequestTest()
        {
            int publisherId = 5;
            IActionResult actionResult = publishersController.DeletePublisherById(publisherId);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }
    }
}