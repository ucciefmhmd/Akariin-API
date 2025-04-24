using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMarketerIdAndTanentIdToBillsRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bills_MarketerId",
                table: "Bills",
                column: "MarketerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_TenantId",
                table: "Bills",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Members_MarketerId",
                table: "Bills",
                column: "MarketerId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Members_TenantId",
                table: "Bills",
                column: "TenantId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Members_MarketerId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Members_TenantId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_MarketerId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_TenantId",
                table: "Bills");
        }
    }
}
