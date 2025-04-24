using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealEstateUnits_Tenant_TenantId1",
                table: "RealEstateUnits");

            migrationBuilder.DropTable(
                name: "Tenant");

            migrationBuilder.DropIndex(
                name: "IX_RealEstateUnits_TenantId1",
                table: "RealEstateUnits");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                table: "RealEstateUnits");

            migrationBuilder.CreateTable(
                name: "MaintenanceRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Property = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CostBearer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaintenanceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrivateMaintenance = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaintenanceRequestFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    RealEstateId = table.Column<long>(type: "bigint", nullable: false),
                    RealEstateUnitId = table.Column<long>(type: "bigint", nullable: false),
                    RealEstateId1 = table.Column<long>(type: "bigint", nullable: true),
                    RealEstateUnitId1 = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Members_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_RealEstateUnits_RealEstateUnitId",
                        column: x => x.RealEstateUnitId,
                        principalTable: "RealEstateUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_RealEstateUnits_RealEstateUnitId1",
                        column: x => x.RealEstateUnitId1,
                        principalTable: "RealEstateUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_RealEstates_RealEstateId",
                        column: x => x.RealEstateId,
                        principalTable: "RealEstates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_RealEstates_RealEstateId1",
                        column: x => x.RealEstateId1,
                        principalTable: "RealEstates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_CreatedById",
                table: "MaintenanceRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_MemberId",
                table: "MaintenanceRequests",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_ModifiedById",
                table: "MaintenanceRequests",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_RealEstateId",
                table: "MaintenanceRequests",
                column: "RealEstateId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_RealEstateId1",
                table: "MaintenanceRequests",
                column: "RealEstateId1");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_RealEstateUnitId",
                table: "MaintenanceRequests",
                column: "RealEstateUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_RealEstateUnitId1",
                table: "MaintenanceRequests",
                column: "RealEstateUnitId1");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_TenantId",
                table: "MaintenanceRequests",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceRequests");

            migrationBuilder.AddColumn<long>(
                name: "TenantId1",
                table: "RealEstateUnits",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenant_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tenant_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateUnits_TenantId1",
                table: "RealEstateUnits",
                column: "TenantId1",
                unique: true,
                filter: "[TenantId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_CreatedById",
                table: "Tenant",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_ModifiedById",
                table: "Tenant",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstateUnits_Tenant_TenantId1",
                table: "RealEstateUnits",
                column: "TenantId1",
                principalTable: "Tenant",
                principalColumn: "Id");
        }
    }
}
