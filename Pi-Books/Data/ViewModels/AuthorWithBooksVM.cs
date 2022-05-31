using System.Collections.Generic;

namespace Pi_Books.Data.ViewModels
{
    public class AuthorWithBooksVM
    {
        public string FullName { get; set; }
        public List<string> BookTitles { get; set; }
    }
}