using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRealEstateUnitTanent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealEstateUnits_Tenant_TenantId",
                table: "RealEstateUnits");

            migrationBuilder.DropIndex(
                name: "IX_RealEstateUnits_TenantId",
                table: "RealEstateUnits");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                table: "RealEstateUnits",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateUnits_TenantId",
                table: "RealEstateUnits",
                column: "TenantId",
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstateUnits_Tenant_TenantId",
                table: "RealEstateUnits",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealEstateUnits_Tenant_TenantId",
                table: "RealEstateUnits");

            migrationBuilder.DropIndex(
                name: "IX_RealEstateUnits_TenantId",
                table: "RealEstateUnits");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                table: "RealEstateUnits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateUnits_TenantId",
                table: "RealEstateUnits",
                column: "TenantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstateUnits_Tenant_TenantId",
                table: "RealEstateUnits",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
