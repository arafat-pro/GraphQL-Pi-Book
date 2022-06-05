using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pi_Books.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewColumnToPublisherTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaxIdentificationNo",
                table: "Publishers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxIdentificationNo",
                table: "Publishers");
        }
    }
}
