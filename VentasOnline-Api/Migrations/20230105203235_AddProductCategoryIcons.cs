using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VentasOnlineApi.Migrations
{
    /// <inheritdoc />
    public partial class AddProductCategoryIcons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconCSS",
                table: "ProductCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconCSS",
                table: "ProductCategories");
        }
    }
}
