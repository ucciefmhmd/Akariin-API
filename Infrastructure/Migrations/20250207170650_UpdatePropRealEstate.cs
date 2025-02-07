using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePropRealEstate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ElectricityCalculation",
                table: "RealEstateUnits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GasMeter",
                table: "RealEstateUnits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "RealEstateUnits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RealEstateId",
                table: "RealEstateUnits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "WaterMeter",
                table: "RealEstateUnits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdNumber",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ElectricityCalculation",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GasMeter",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guard",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GuardId",
                table: "RealEstates",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuardMobile",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "RealEstates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaterMeter",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealEstateUnits_RealEstateId",
                table: "RealEstateUnits",
                column: "RealEstateId");

            migrationBuilder.AddForeignKey(
                name: "FK_RealEstateUnits_RealEstates_RealEstateId",
                table: "RealEstateUnits",
                column: "RealEstateId",
                principalTable: "RealEstates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealEstateUnits_RealEstates_RealEstateId",
                table: "RealEstateUnits");

            migrationBuilder.DropIndex(
                name: "IX_RealEstateUnits_RealEstateId",
                table: "RealEstateUnits");

            migrationBuilder.DropColumn(
                name: "ElectricityCalculation",
                table: "RealEstateUnits");

            migrationBuilder.DropColumn(
                name: "GasMeter",
                table: "RealEstateUnits");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "RealEstateUnits");

            migrationBuilder.DropColumn(
                name: "RealEstateId",
                table: "RealEstateUnits");

            migrationBuilder.DropColumn(
                name: "WaterMeter",
                table: "RealEstateUnits");

            migrationBuilder.DropColumn(
                name: "AdNumber",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "ElectricityCalculation",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "GasMeter",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "Guard",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "GuardId",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "GuardMobile",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "WaterMeter",
                table: "RealEstates");
        }
    }
}
