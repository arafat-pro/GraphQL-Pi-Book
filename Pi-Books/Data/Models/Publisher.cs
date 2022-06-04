using System.Collections.Generic;
using HotChocolate;

namespace Pi_Books.Data.Models
{
    [GraphQLDescription("Represents a business entity for who commercially publishes Books.")]
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigation Properties
        public List<Book> Books { get; set; }
    }
}
