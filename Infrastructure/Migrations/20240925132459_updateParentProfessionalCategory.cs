using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateParentProfessionalCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_ProfessionalCategories_ProfessionalCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalCategories_ProductCategories_ParentCategoryId",
                table: "ProfessionalCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_ProfessionalCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "ProfessionalCategoryId",
                table: "ProductCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionalCategories_ProfessionalCategories_ParentCategoryId",
                table: "ProfessionalCategories",
                column: "ParentCategoryId",
                principalTable: "ProfessionalCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalCategories_ProfessionalCategories_ParentCategoryId",
                table: "ProfessionalCategories");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfessionalCategoryId",
                table: "ProductCategories",
                type: "uniqueidentifier",
                nullable: true);

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
        }
    }
}
