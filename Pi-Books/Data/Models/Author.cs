using System.Collections.Generic;

namespace Pi_Books.Data.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigation Properties
        public List<BookAuthor> BookAuthors { get; set; }
    }
}
