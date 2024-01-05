using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class fixTheRelationBetweenUserAndOrderHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHistory_AspNetUsers_EditorUserId1",
                table: "OrderHistory");

            migrationBuilder.DropIndex(
                name: "IX_OrderHistory_EditorUserId1",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "EditorUserId1",
                table: "OrderHistory");

            migrationBuilder.AlterColumn<string>(
                name: "EditorUserId",
                table: "OrderHistory",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_EditorUserId",
                table: "OrderHistory",
                column: "EditorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHistory_AspNetUsers_EditorUserId",
                table: "OrderHistory",
                column: "EditorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHistory_AspNetUsers_EditorUserId",
                table: "OrderHistory");

            migrationBuilder.DropIndex(
                name: "IX_OrderHistory_EditorUserId",
                table: "OrderHistory");

            migrationBuilder.AlterColumn<int>(
                name: "EditorUserId",
                table: "OrderHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "EditorUserId1",
                table: "OrderHistory",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_EditorUserId1",
                table: "OrderHistory",
                column: "EditorUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHistory_AspNetUsers_EditorUserId1",
                table: "OrderHistory",
                column: "EditorUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
