using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductSizeCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductSizes_ProductSizeId",
                table: "CartItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductSizeId",
                table: "CartItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductSizes_ProductSizeId",
                table: "CartItems",
                column: "ProductSizeId",
                principalTable: "ProductSizes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductSizes_ProductSizeId",
                table: "CartItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductSizeId",
                table: "CartItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductSizes_ProductSizeId",
                table: "CartItems",
                column: "ProductSizeId",
                principalTable: "ProductSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
