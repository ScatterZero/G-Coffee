using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G_Cofee_Repositories.Migrations
{
    /// <inheritdoc />
    public partial class MIG1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TentUserId",
                table: "Transactions",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TentUserId",
                table: "TransactionDetails",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TentUserId",
                table: "Suppliers",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TentUserId",
                table: "Products",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TentUserId",
                table: "Inventory",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TentUserId",
                table: "Transactions",
                column: "TentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_TentUserId",
                table: "TransactionDetails",
                column: "TentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_TentUserId",
                table: "Suppliers",
                column: "TentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TentUserId",
                table: "Products",
                column: "TentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_TentUserId",
                table: "Inventory",
                column: "TentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Users_TentUserId",
                table: "Inventory",
                column: "TentUserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_TentUserId",
                table: "Products",
                column: "TentUserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Users_TentUserId",
                table: "Suppliers",
                column: "TentUserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_Users_TentUserId",
                table: "TransactionDetails",
                column: "TentUserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_TentUserId",
                table: "Transactions",
                column: "TentUserId",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Users_TentUserId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_TentUserId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Users_TentUserId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_Users_TentUserId",
                table: "TransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_TentUserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TentUserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_TransactionDetails_TentUserId",
                table: "TransactionDetails");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_TentUserId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Products_TentUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_TentUserId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "TentUserId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TentUserId",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "TentUserId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "TentUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TentUserId",
                table: "Inventory");
        }
    }
}
