using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMarketerIdAndTanentIdToBills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketerIdNumber",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "MarketerIdType",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "MarketerName",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "MarketerPhoneNumber",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "TenantAddress",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "TenantIdNumber",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "TenantIdType",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "TenantName",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "TenantPhoneNumber",
                table: "Bills");

            migrationBuilder.AddColumn<decimal>(
                name: "ConfirmSalary",
                table: "Bills",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MarketerId",
                table: "Bills",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "StatusBills",
                table: "Bills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "Bills",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmSalary",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "MarketerId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "StatusBills",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Bills");

            migrationBuilder.AddColumn<string>(
                name: "MarketerIdNumber",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketerIdType",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketerName",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketerPhoneNumber",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantAddress",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantIdNumber",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantIdType",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantName",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenantPhoneNumber",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
