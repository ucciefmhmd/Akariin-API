using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProfessionalCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentCategoryId",
                table: "ProfessionalCategories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfessionalCategoryId",
                table: "ProductCategories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientId",
                table: "Projects",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalCategories_ParentCategoryId",
                table: "ProfessionalCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProfessionalCategoryId",
                table: "ProductCategories",
                column: "ProfessionalCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_ProfessionalCategories_ProfessionalCategoryId",
                table: "ProductCategories",
                column: "ProfessionalCategoryId",
                principalTable: "ProfessionalCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionalCategories_ProductCategories_ParentCategoryId",
                table: "ProfessionalCategories",
                column: "ParentCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_ClientId",
                table: "Projects",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_ProfessionalCategories_ProfessionalCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalCategories_ProductCategories_ParentCategoryId",
                table: "ProfessionalCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_ClientId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ClientId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_ProfessionalCategories_ParentCategoryId",
                table: "ProfessionalCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_ProfessionalCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "ProfessionalCategories");

            migrationBuilder.DropColumn(
                name: "ProfessionalCategoryId",
                table: "ProductCategories");
        }
    }
}
