using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Pi_Books.Data.Models;
using Pi_Books.Data.Paging;
using Pi_Books.Data.ViewModels;
using Pi_Books.Exceptions;

namespace Pi_Books.Data.Services
{
    public class PublishersService
    {
        private AppDbContext _context;
        public PublishersService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public List<Publisher> GetAllPublishers(string sortBy, string searchText, int? pageNumber)
        {
            var allPublishers = _context.Publishers.OrderBy(n => n.Name).ToList();

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        allPublishers = allPublishers.OrderByDescending(n => n.Name).ToList();
                        break;

                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                allPublishers = allPublishers
                    .Where(n => n.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
            }

            int pageSize = 4;
            allPublishers = PaginatedList<Publisher>.Create(allPublishers.AsQueryable(), pageNumber ?? 1, pageSize);

            return allPublishers;
        }

        public Publisher AddPublisher(PublisherVM publisher)
        {
            if (StringStartWithNumber(publisher.Name)) throw new PublisherNameException("Name Started with Number!", publisher.Name);

            var _publisher = new Publisher()
            {
                Name = publisher.Name
            };
            _context.Publishers.Add(_publisher);
            _context.SaveChanges();

            return _publisher;
        }

        public Publisher GetThePublisher(int publisherId) => _context.Publishers.FirstOrDefault(p => p.Id == publisherId);

        public PublisherWithBooksAndAuthorsVM GetAllBooksAndAuthorsOfPublisher(int publisherId)
        {
            var _booksAndAuthors = _context.Publishers.Where(p => p.Id == publisherId).Select(ba => new PublisherWithBooksAndAuthorsVM()
            {
                Name = ba.Name,
                BookAuthors = ba.Books.Select(x => new BookAuthorVM()
                {
                    BookName = x.Title,
                    BookAuthors = x.BookAuthors.Select(a => a.Author.Name).ToList()
                }).ToList()
            }).FirstOrDefault();

            return _booksAndAuthors;
        }

        public void DeleteThePublisher(int publisherId)
        {
            var publisherInDb = _context.Publishers.FirstOrDefault(p => p.Id == publisherId);

            if (publisherInDb != null)
            {
                _context.Publishers.Remove(publisherInDb);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"The Publisher Id requested - {publisherId} doesn't exist!");
            }
        }

        //private bool StringStartWithNumber (string nameOfPublisher)
        //{
        //    if (Regex.IsMatch(nameOfPublisher, @"^\d")) return true;
        //    return false;
        //}

        //private bool StringStartWithNumber(string nameOfPublisher)
        //{
        //    return (Regex.IsMatch(nameOfPublisher, @"^\d"));
        //}

        private bool StringStartWithNumber(string nameOfPublisher) => (Regex.IsMatch(nameOfPublisher, @"^\d"));

    }
}