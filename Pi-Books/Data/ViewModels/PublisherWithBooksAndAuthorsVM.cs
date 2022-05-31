using System.Collections.Generic;

namespace Pi_Books.Data.ViewModels
{
    public class PublisherWithBooksAndAuthorsVM
    {
        public string Name { get; set; }
        public List<BookAuthorVM> BookAuthors { get; set; }
    }
}