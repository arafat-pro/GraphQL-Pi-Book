using GraphQL.Types;
using Pi_Books.Data.Services;
using Pi_Books.GraphQL.Types;

namespace Pi_Books.GraphQL.Queries
{
    public class BookQuery : ObjectGraphType
    {
        public BookQuery(BooksService booksServices)
        {
            Field<ListGraphType<BookType>>(
                "books",
                "Returns list of books",
                resolve: context => booksServices.GetAllBooks()
                );
        }
    }
}