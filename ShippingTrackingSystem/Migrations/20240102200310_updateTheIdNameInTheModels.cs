using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class updateTheIdNameInTheModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Categories",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categories",
                newName: "ID");
        }
    }
}
