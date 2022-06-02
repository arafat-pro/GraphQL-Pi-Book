using GraphQL;
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

            Field<BookType>(
                    "book",
                    "Returns an specific book by id.",
                    new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "Book Id" }),
                resolve: context => booksServices.GetTheBook(context.GetArgument("id", int.MinValue)));
        }
    }
}