using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_inventories_InventoryId",
                table: "InventoryItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem");

            migrationBuilder.RenameTable(
                name: "InventoryItem",
                newName: "InventoryItems");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItem_InventoryId",
                table: "InventoryItems",
                newName: "IX_InventoryItems_InventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_inventories_InventoryId",
                table: "InventoryItems",
                column: "InventoryId",
                principalTable: "inventories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_inventories_InventoryId",
                table: "InventoryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems");

            migrationBuilder.RenameTable(
                name: "InventoryItems",
                newName: "InventoryItem");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItems_InventoryId",
                table: "InventoryItem",
                newName: "IX_InventoryItem_InventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_inventories_InventoryId",
                table: "InventoryItem",
                column: "InventoryId",
                principalTable: "inventories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
