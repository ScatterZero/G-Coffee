using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G_Cofee_Repositories.Migrations
{
    /// <inheritdoc />
    public partial class MIGG2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantID",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantID",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantID",
                table: "TransactionDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantID",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantID",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantID",
                table: "Inventory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TenantID",
                table: "Inventory");
        }
    }
}
