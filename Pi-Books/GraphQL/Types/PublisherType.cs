using HotChocolate.Types;
using Pi_Books.Data;
using Pi_Books.Data.Models;

namespace Pi_Books.GraphQL.Types
{
    public partial class PublisherType : ObjectType<Publisher>
    {
        protected override void Configure(IObjectTypeDescriptor<Publisher> descriptor)
        {
            descriptor.Description("Represents a business entity for who commercially publishes Books.");

            descriptor.Field(p => p.TaxIdentificationNo).Ignore();

            descriptor
                .Field(p => p.Books)
                .ResolveWith<Resolvers>(b => b.GetBooks(default!, default!))
                .UseDbContext<AppDbGraphQLContext>()
                .Description("This is the list of available books for this publisher!");
        }
    }
}