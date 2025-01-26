using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePrivacyModelAddTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contacts",
                table: "Privacies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cookies",
                table: "Privacies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CopyRights",
                table: "Privacies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PrivacyLocalizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrivacyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Terms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cookies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CopyRights = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contacts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivacyLocalizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivacyLocalizations_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrivacyLocalizations_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrivacyLocalizations_Privacies_PrivacyId",
                        column: x => x.PrivacyId,
                        principalTable: "Privacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivacyLocalizations_CreatedById",
                table: "PrivacyLocalizations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PrivacyLocalizations_ModifiedById",
                table: "PrivacyLocalizations",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_PrivacyLocalizations_PrivacyId",
                table: "PrivacyLocalizations",
                column: "PrivacyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivacyLocalizations");

            migrationBuilder.DropColumn(
                name: "Contacts",
                table: "Privacies");

            migrationBuilder.DropColumn(
                name: "Cookies",
                table: "Privacies");

            migrationBuilder.DropColumn(
                name: "CopyRights",
                table: "Privacies");
        }
    }
}
