using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G_Cofee_Repositories.Migrations
{
    /// <inheritdoc />
    public partial class MIGG22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Payments__Create__7A672E12",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK__Payments__Update__7B5B524B",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK__Products__Create__5165187F",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK__Products__Update__52593CB8",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Creat__656C112C",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK__Transacti__Updat__66603565",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK__Warehouse__Manag__3D5E1FD2",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CreatedBy",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_UpdatedBy",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Products_CreatedBy",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UpdatedBy",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CreatedBy",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UpdatedBy",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByNavigationUserId",
                table: "Transactions",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByNavigationUserId",
                table: "Transactions",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByNavigationUserId",
                table: "Products",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByNavigationUserId",
                table: "Products",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByNavigationUserId",
                table: "Payments",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByNavigationUserId",
                table: "Payments",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedByNavigationUserId",
                table: "Transactions",
                column: "CreatedByNavigationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UpdatedByNavigationUserId",
                table: "Transactions",
                column: "UpdatedByNavigationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedByNavigationUserId",
                table: "Products",
                column: "CreatedByNavigationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UpdatedByNavigationUserId",
                table: "Products",
                column: "UpdatedByNavigationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreatedByNavigationUserId",
                table: "Payments",
                column: "CreatedByNavigationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UpdatedByNavigationUserId",
                table: "Payments",
                column: "UpdatedByNavigationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_CreatedByNavigationUserId",
                table: "Payments",
                column: "CreatedByNavigationUserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UpdatedByNavigationUserId",
                table: "Payments",
                column: "UpdatedByNavigationUserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_CreatedByNavigationUserId",
                table: "Products",
                column: "CreatedByNavigationUserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_UpdatedByNavigationUserId",
                table: "Products",
                column: "UpdatedByNavigationUserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_CreatedByNavigationUserId",
                table: "Transactions",
                column: "CreatedByNavigationUserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_UpdatedByNavigationUserId",
                table: "Transactions",
                column: "UpdatedByNavigationUserId",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_Users_ManagerID",
                table: "Warehouses",
                column: "ManagerID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_CreatedByNavigationUserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UpdatedByNavigationUserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_CreatedByNavigationUserId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_UpdatedByNavigationUserId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_CreatedByNavigationUserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_UpdatedByNavigationUserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_Users_ManagerID",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CreatedByNavigationUserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_UpdatedByNavigationUserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Products_CreatedByNavigationUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UpdatedByNavigationUserId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CreatedByNavigationUserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UpdatedByNavigationUserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationUserId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationUserId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedByNavigationUserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationUserId",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedBy",
                table: "Transactions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UpdatedBy",
                table: "Transactions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedBy",
                table: "Products",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UpdatedBy",
                table: "Products",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreatedBy",
                table: "Payments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UpdatedBy",
                table: "Payments",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK__Payments__Create__7A672E12",
                table: "Payments",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Payments__Update__7B5B524B",
                table: "Payments",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Products__Create__5165187F",
                table: "Products",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Products__Update__52593CB8",
                table: "Products",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Creat__656C112C",
                table: "Transactions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Transacti__Updat__66603565",
                table: "Transactions",
                column: "UpdatedBy",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Warehouse__Manag__3D5E1FD2",
                table: "Warehouses",
                column: "ManagerID",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}
