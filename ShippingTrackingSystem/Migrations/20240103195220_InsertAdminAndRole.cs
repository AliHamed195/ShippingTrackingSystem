using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class InsertAdminAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //admin@admin.com
            //123456
            //Admin, Customer, Delivery, Warehouse
            migrationBuilder.Sql("INSERT INTO dbo.AspNetRoles(Id,Name,NormalizedName) VALUES('bb0ddbff-0ae6-46da-ae65-84c3bfae72fd', 'Admin', 'ADMIN')");
            migrationBuilder.Sql($@"
            INSERT INTO dbo.AspNetUsers(
                Id, 
                UserName, 
                NormalizedUserName,  
                Email, 
                NormalizedEmail, 
                EmailConfirmed,
                PhoneNumberConfirmed, 
                LockoutEnabled,
                TwoFactorEnabled, 
                AccessFailedCount, 
                PasswordHash,
                SecurityStamp,
                Address,
                CreatedOn,
                Discriminator
            ) 
            VALUES(
                '8235366b-3196-4dc5-9cd8-fb4850de8e80',
                'admin@admin.com',
                'ADMIN@ADMIN.COM',
                'admin@admin.com',
                'ADMIN@ADMIN.COM',
                1,
                0,
                0,
                0,
                0,
                'AQAAAAIAAYagAAAAEFqhrE3IWp6vkn5TYXnXcqo5js7hWk93RYBURaF0BFMZL/7oiktO/rh9Vw5e2YhLXw==',
                '1135d8d8-0a2c-473c-bd40-afc934082779',
                'Lebanon',
                '{DateTime.Now}',
                'ApplicationUser'
            )");
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUserRoles]([UserId],[RoleId])VALUES('8235366b-3196-4dc5-9cd8-fb4850de8e80','bb0ddbff-0ae6-46da-ae65-84c3bfae72fd')");
            migrationBuilder.Sql("INSERT INTO dbo.AspNetRoles(Id,Name,NormalizedName) VALUES('3c48f50c-7f36-449a-b5c3-8915ab888e92', 'Customer', 'CUSTOMER')");
            migrationBuilder.Sql("INSERT INTO dbo.AspNetRoles(Id,Name,NormalizedName) VALUES('eea0ec7b-7ecb-4a64-a551-158c9c4cb285', 'Delivery', 'DELIVERY')");
            migrationBuilder.Sql("INSERT INTO dbo.AspNetRoles(Id,Name,NormalizedName) VALUES('af469a30-afb7-4b7f-ade8-8f2a0cfdcf69', 'Warehouse', 'WAREHOUSE')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles] WHERE Id = 'bb0ddbff-0ae6-46da-ae65-84c3bfae72fd'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUsers] WHERE Id = '8235366b-3196-4dc5-9cd8-fb4850de8e80'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles] WHERE Id = '3c48f50c-7f36-449a-b5c3-8915ab888e92'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles] WHERE Id = 'eea0ec7b-7ecb-4a64-a551-158c9c4cb285'");
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetRoles] WHERE Id = 'af469a30-afb7-4b7f-ade8-8f2a0cfdcf69'");
        }
    }
}
