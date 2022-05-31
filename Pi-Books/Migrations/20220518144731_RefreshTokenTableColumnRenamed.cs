using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pi_Books.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenTableColumnRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateExpired",
                table: "RefreshTokens",
                newName: "DateExpire");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateExpire",
                table: "RefreshTokens",
                newName: "DateExpired");
        }
    }
}
