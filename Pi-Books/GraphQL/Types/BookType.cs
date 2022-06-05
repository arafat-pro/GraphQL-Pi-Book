using HotChocolate.Types;
using Pi_Books.Data;
using Pi_Books.Data.Models;

namespace Pi_Books.GraphQL.Types
{
    public partial class BookType : ObjectType<Book>
    {
        protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
        {
            descriptor
                .Description("A written or printed work consisting of pages glued or sewn together along one side and bound in covers.");

            descriptor.Field(b => b.Rating)
                .Description("The Popularity of the book in the scale of 1 to 10.");


            descriptor.Field(b => b.Publisher)
                .ResolveWith<Resolver>(b => b.GetPublisher(default!, default!))
                .UseDbContext<AppDbGraphQLContext>()
                .Description("This is the Publisher by which the Book is published!");
        }
    }
}