using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFilterForProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductFilters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFilters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFilters_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductFilters_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductFilters_ProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductFilterOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFilterOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFilterOptions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductFilterOptions_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductFilterOptions_ProductFilters_FilterId",
                        column: x => x.FilterId,
                        principalTable: "ProductFilters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsFilterOptions",
                columns: table => new
                {
                    FilterOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsFilterOptions", x => new { x.ProductId, x.FilterOptionId });
                    table.ForeignKey(
                        name: "FK_ProductsFilterOptions_ProductFilterOptions_FilterOptionId",
                        column: x => x.FilterOptionId,
                        principalTable: "ProductFilterOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsFilterOptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductFilterOptions_CreatedById",
                table: "ProductFilterOptions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFilterOptions_FilterId",
                table: "ProductFilterOptions",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFilterOptions_ModifiedById",
                table: "ProductFilterOptions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFilters_CategoryId",
                table: "ProductFilters",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFilters_CreatedById",
                table: "ProductFilters",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFilters_ModifiedById",
                table: "ProductFilters",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsFilterOptions_FilterOptionId",
                table: "ProductsFilterOptions",
                column: "FilterOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsFilterOptions");

            migrationBuilder.DropTable(
                name: "ProductFilterOptions");

            migrationBuilder.DropTable(
                name: "ProductFilters");
        }
    }
}
