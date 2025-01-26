using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductID",
                table: "ProductReviews");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "ProductReviews",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_ProductID",
                table: "ProductReviews",
                newName: "IX_ProductReviews_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductReviews",
                newName: "ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductID",
                table: "ProductReviews",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
