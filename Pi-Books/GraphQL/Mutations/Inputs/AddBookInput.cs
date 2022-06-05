namespace Pi_Books.GraphQL.Mutations.Inputs
{
    public record AddBookInput(
        string Title,
        string Genre,
        int PublisherId
        );
}