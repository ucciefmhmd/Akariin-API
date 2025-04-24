using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBillData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Bills",
                newName: "BillNumber");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Bills",
                newName: "BillDate");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Bills",
                newName: "TotalAmount");

            migrationBuilder.AddColumn<float>(
                name: "PaymentAmount",
                table: "Contracts",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<float>(
                name: "Tax",
                table: "Bills",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "Discount",
                table: "Bills",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<string>(
                name: "IssuedBy",
                table: "Bills",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentAmount",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "IssuedBy",
                table: "Bills");

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

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Bills",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "BillNumber",
                table: "Bills",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "BillDate",
                table: "Bills",
                newName: "Date");

            migrationBuilder.AlterColumn<float>(
                name: "Tax",
                table: "Bills",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Discount",
                table: "Bills",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
