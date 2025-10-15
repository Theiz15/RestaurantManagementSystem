using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_users_cashier_id",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "Payments",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "cashier_id",
                table: "Payments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_cashier_id",
                table: "Payments",
                newName: "IX_Payments_UserId");

            migrationBuilder.AddColumn<string>(
                name: "RawData",
                table: "Payments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "Payments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VnpResponseCode",
                table: "Payments",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "VnpTransactionNo",
                table: "Payments",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_users_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_users_UserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RawData",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "VnpResponseCode",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "VnpTransactionNo",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Payments",
                newName: "cashier_id");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "Payments",
                newName: "timestamp");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                newName: "IX_Payments_cashier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_users_cashier_id",
                table: "Payments",
                column: "cashier_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
