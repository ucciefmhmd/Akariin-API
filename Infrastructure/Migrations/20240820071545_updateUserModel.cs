using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CirtificationAndAwards",
                table: "AspNetUsers",
                newName: "CertificationAndAwards");

            migrationBuilder.RenameColumn(
                name: "BuissnessDescription",
                table: "AspNetUsers",
                newName: "BuisnessDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CertificationAndAwards",
                table: "AspNetUsers",
                newName: "CirtificationAndAwards");

            migrationBuilder.RenameColumn(
                name: "BuisnessDescription",
                table: "AspNetUsers",
                newName: "BuissnessDescription");
        }
    }
}
