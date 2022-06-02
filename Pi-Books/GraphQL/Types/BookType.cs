using GraphQL.Types;
using Pi_Books.Data.Models;

namespace Pi_Books.GraphQL.Types
{
    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Field(x => x.Id, type: typeof(IdGraphType))
                .Description("Id Property from the Book Model/Object.");

            Field(x => x.Title, type: typeof(StringGraphType))
                .Description("Title Property from the Book Model/Object.");

            Field(x => x.Description, type: typeof(StringGraphType))
                .Description("Description Property from the Book Model/Object.");

            Field(x => x.Rating, type: typeof(IntGraphType))
                .Description("Rating Property from the Book Model/Object.");

            Field(x => x.DateAdded, type: typeof(DateTimeGraphType))
                .Description("Date Added Property from the Book Model/Object.");

            Field(x => x.DateRead, type: typeof(DateTimeGraphType))
                .Description("Date Read Property from the Book Model/Object.");
        }
    }
}