using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pi_Books.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGenere2Genre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Genere",
                table: "Books",
                newName: "Genre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Books",
                newName: "Genere");
        }
    }
}
