using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pi_Books.Migrations
{
    /// <inheritdoc />
    public partial class RemovedFkColumnFromPublisherTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Publishers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Publishers",
                type: "int",
                nullable: true);
        }
    }
}
