using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFluentApiNoAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Members_MarketerId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Members_TenantId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_RealEstateUnits_RealEstateUnitId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_Members_MemberId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_Members_TenantId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_RealEstateUnits_RealEstateUnitId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_RealEstates_RealEstateId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RealEstates_Members_OwnerId",
                table: "RealEstates");

            migrationBuilder.DropForeignKey(
                name: "FK_RealEstateUnits_Members_TenantId",
                table: "RealEstateUnits");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Members_MarketerId",
                table: "Contracts",
                column: "MarketerId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Members_TenantId",
                table: "Contracts",
                column: "TenantId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_RealEstateUnits_RealEstateUnitId",
                table: "Contracts",
                column: "RealEstateUnitId",
                principalTable: "RealEstateUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_Members_MemberId",
                table: "MaintenanceRequests",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_Members_TenantId",
                table: "MaintenanceRequests",
                column: "TenantId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_RealEstateUnits_RealEstateUnitId",
                table: "MaintenanceRequests",
                column: "RealEstateUnitId",
                principalTable: "RealEstateUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_RealEstates_RealEstateId",
                table: "MaintenanceRequests",
                column: "RealEstateId",
                principalTable: "RealEstates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstates_Members_OwnerId",
                table: "RealEstates",
                column: "OwnerId",
                principalTable: "Members",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstateUnits_Members_TenantId",
                table: "RealEstateUnits",
                column: "TenantId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Members_MarketerId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Members_TenantId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_RealEstateUnits_RealEstateUnitId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_Members_MemberId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_Members_TenantId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_RealEstateUnits_RealEstateUnitId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_RealEstates_RealEstateId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RealEstates_Members_OwnerId",
                table: "RealEstates");

            migrationBuilder.DropForeignKey(
                name: "FK_RealEstateUnits_Members_TenantId",
                table: "RealEstateUnits");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Members_MarketerId",
                table: "Contracts",
                column: "MarketerId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Members_TenantId",
                table: "Contracts",
                column: "TenantId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_RealEstateUnits_RealEstateUnitId",
                table: "Contracts",
                column: "RealEstateUnitId",
                principalTable: "RealEstateUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_Members_MemberId",
                table: "MaintenanceRequests",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_Members_TenantId",
                table: "MaintenanceRequests",
                column: "TenantId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_RealEstateUnits_RealEstateUnitId",
                table: "MaintenanceRequests",
                column: "RealEstateUnitId",
                principalTable: "RealEstateUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_RealEstates_RealEstateId",
                table: "MaintenanceRequests",
                column: "RealEstateId",
                principalTable: "RealEstates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstates_Members_OwnerId",
                table: "RealEstates",
                column: "OwnerId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstateUnits_Members_TenantId",
                table: "RealEstateUnits",
                column: "TenantId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
