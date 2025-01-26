using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBillingInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlternativePhoneNumber",
                table: "ShippingAddresses");

            migrationBuilder.DropColumn(
                name: "FamilyName",
                table: "ShippingAddresses");

            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "ShippingAddresses");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ShippingAddresses");

            migrationBuilder.DropColumn(
                name: "FullNameInEnglish",
                table: "ShippingAddresses");

            migrationBuilder.DropColumn(
                name: "MainPhoneNumber",
                table: "ShippingAddresses");

            migrationBuilder.AddColumn<string>(
                name: "BillingAlternativePhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingFamilyName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingFatherName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingFirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingFullNameInEnglish",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingMainPhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAlternativePhoneNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BillingFamilyName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BillingFatherName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BillingFirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BillingFullNameInEnglish",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BillingMainPhoneNumber",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AlternativePhoneNumber",
                table: "ShippingAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FamilyName",
                table: "ShippingAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "ShippingAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ShippingAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullNameInEnglish",
                table: "ShippingAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainPhoneNumber",
                table: "ShippingAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
