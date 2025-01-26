using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProductModelsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_CreatedById",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_ModifiedById",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_UserID",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_AspNetUsers_CreatedById",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_AspNetUsers_ModifiedById",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Order_OrderID",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Product_ProductID",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_CompanyID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_CreatedById",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_ModifiedById",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategory_CategoryID",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Product_ProductId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_AspNetUsers_CreatedById",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_AspNetUsers_ModifiedById",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_ProductCategory_ParentCategoryID",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_AspNetUsers_CreatedById",
                table: "ProductReview");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_AspNetUsers_ModifiedById",
                table: "ProductReview");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_AspNetUsers_UserID",
                table: "ProductReview");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_Product_ProductID",
                table: "ProductReview");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecification_AspNetUsers_CreatedById",
                table: "ProductSpecification");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecification_AspNetUsers_ModifiedById",
                table: "ProductSpecification");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecification_Product_ProductID",
                table: "ProductSpecification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSpecification",
                table: "ProductSpecification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReview",
                table: "ProductReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "ProductSpecification",
                newName: "ProductSpecifications");

            migrationBuilder.RenameTable(
                name: "ProductReview",
                newName: "ProductReviews");

            migrationBuilder.RenameTable(
                name: "ProductCategory",
                newName: "ProductCategories");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "OrderItem",
                newName: "OrderItems");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecification_ProductID",
                table: "ProductSpecifications",
                newName: "IX_ProductSpecifications_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecification_ModifiedById",
                table: "ProductSpecifications",
                newName: "IX_ProductSpecifications_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecification_CreatedById",
                table: "ProductSpecifications",
                newName: "IX_ProductSpecifications_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReview_UserID",
                table: "ProductReviews",
                newName: "IX_ProductReviews_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReview_ProductID",
                table: "ProductReviews",
                newName: "IX_ProductReviews_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReview_ModifiedById",
                table: "ProductReviews",
                newName: "IX_ProductReviews_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReview_CreatedById",
                table: "ProductReviews",
                newName: "IX_ProductReviews_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_ParentCategoryID",
                table: "ProductCategories",
                newName: "IX_ProductCategories_ParentCategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_ModifiedById",
                table: "ProductCategories",
                newName: "IX_ProductCategories_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_CreatedById",
                table: "ProductCategories",
                newName: "IX_ProductCategories_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ProductId",
                table: "Products",
                newName: "IX_Products_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ModifiedById",
                table: "Products",
                newName: "IX_Products_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CreatedById",
                table: "Products",
                newName: "IX_Products_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CompanyID",
                table: "Products",
                newName: "IX_Products_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryID",
                table: "Products",
                newName: "IX_Products_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_ProductID",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_OrderID",
                table: "OrderItems",
                newName: "IX_OrderItems_OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_ModifiedById",
                table: "OrderItems",
                newName: "IX_OrderItems_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_CreatedById",
                table: "OrderItems",
                newName: "IX_OrderItems_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Order_UserID",
                table: "Orders",
                newName: "IX_Orders_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Order_ModifiedById",
                table: "Orders",
                newName: "IX_Orders_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CreatedById",
                table: "Orders",
                newName: "IX_Orders_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSpecifications",
                table: "ProductSpecifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_AspNetUsers_CreatedById",
                table: "OrderItems",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_AspNetUsers_ModifiedById",
                table: "OrderItems",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderID",
                table: "OrderItems",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductID",
                table: "OrderItems",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CreatedById",
                table: "Orders",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_ModifiedById",
                table: "Orders",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserID",
                table: "Orders",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_AspNetUsers_CreatedById",
                table: "ProductCategories",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_AspNetUsers_ModifiedById",
                table: "ProductCategories",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_ProductCategories_ParentCategoryID",
                table: "ProductCategories",
                column: "ParentCategoryID",
                principalTable: "ProductCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_AspNetUsers_CreatedById",
                table: "ProductReviews",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_AspNetUsers_ModifiedById",
                table: "ProductReviews",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_AspNetUsers_UserID",
                table: "ProductReviews",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductID",
                table: "ProductReviews",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_CompanyID",
                table: "Products",
                column: "CompanyID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_CreatedById",
                table: "Products",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_ModifiedById",
                table: "Products",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Products_ProductId",
                table: "Products",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecifications_AspNetUsers_CreatedById",
                table: "ProductSpecifications",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecifications_AspNetUsers_ModifiedById",
                table: "ProductSpecifications",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecifications_Products_ProductID",
                table: "ProductSpecifications",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_AspNetUsers_CreatedById",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_AspNetUsers_ModifiedById",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderID",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductID",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CreatedById",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_ModifiedById",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_AspNetUsers_CreatedById",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_AspNetUsers_ModifiedById",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_ProductCategories_ParentCategoryID",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_AspNetUsers_CreatedById",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_AspNetUsers_ModifiedById",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_AspNetUsers_UserID",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductID",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_CompanyID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_CreatedById",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_ModifiedById",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategories_CategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Products_ProductId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecifications_AspNetUsers_CreatedById",
                table: "ProductSpecifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecifications_AspNetUsers_ModifiedById",
                table: "ProductSpecifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSpecifications_Products_ProductID",
                table: "ProductSpecifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSpecifications",
                table: "ProductSpecifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategories",
                table: "ProductCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.RenameTable(
                name: "ProductSpecifications",
                newName: "ProductSpecification");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "ProductReviews",
                newName: "ProductReview");

            migrationBuilder.RenameTable(
                name: "ProductCategories",
                newName: "ProductCategory");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "OrderItem");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecifications_ProductID",
                table: "ProductSpecification",
                newName: "IX_ProductSpecification_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecifications_ModifiedById",
                table: "ProductSpecification",
                newName: "IX_ProductSpecification_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSpecifications_CreatedById",
                table: "ProductSpecification",
                newName: "IX_ProductSpecification_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ProductId",
                table: "Product",
                newName: "IX_Product_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ModifiedById",
                table: "Product",
                newName: "IX_Product_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CreatedById",
                table: "Product",
                newName: "IX_Product_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CompanyID",
                table: "Product",
                newName: "IX_Product_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryID",
                table: "Product",
                newName: "IX_Product_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_UserID",
                table: "ProductReview",
                newName: "IX_ProductReview_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_ProductID",
                table: "ProductReview",
                newName: "IX_ProductReview_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_ModifiedById",
                table: "ProductReview",
                newName: "IX_ProductReview_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_CreatedById",
                table: "ProductReview",
                newName: "IX_ProductReview_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_ParentCategoryID",
                table: "ProductCategory",
                newName: "IX_ProductCategory_ParentCategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_ModifiedById",
                table: "ProductCategory",
                newName: "IX_ProductCategory_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategories_CreatedById",
                table: "ProductCategory",
                newName: "IX_ProductCategory_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserID",
                table: "Order",
                newName: "IX_Order_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ModifiedById",
                table: "Order",
                newName: "IX_Order_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CreatedById",
                table: "Order",
                newName: "IX_Order_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductID",
                table: "OrderItem",
                newName: "IX_OrderItem_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_OrderID",
                table: "OrderItem",
                newName: "IX_OrderItem_OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ModifiedById",
                table: "OrderItem",
                newName: "IX_OrderItem_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_CreatedById",
                table: "OrderItem",
                newName: "IX_OrderItem_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSpecification",
                table: "ProductSpecification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReview",
                table: "ProductReview",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_CreatedById",
                table: "Order",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_ModifiedById",
                table: "Order",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_UserID",
                table: "Order",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_AspNetUsers_CreatedById",
                table: "OrderItem",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_AspNetUsers_ModifiedById",
                table: "OrderItem",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order_OrderID",
                table: "OrderItem",
                column: "OrderID",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Product_ProductID",
                table: "OrderItem",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_CompanyID",
                table: "Product",
                column: "CompanyID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_CreatedById",
                table: "Product",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_ModifiedById",
                table: "Product",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategory_CategoryID",
                table: "Product",
                column: "CategoryID",
                principalTable: "ProductCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Product_ProductId",
                table: "Product",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_AspNetUsers_CreatedById",
                table: "ProductCategory",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_AspNetUsers_ModifiedById",
                table: "ProductCategory",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_ProductCategory_ParentCategoryID",
                table: "ProductCategory",
                column: "ParentCategoryID",
                principalTable: "ProductCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_AspNetUsers_CreatedById",
                table: "ProductReview",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_AspNetUsers_ModifiedById",
                table: "ProductReview",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_AspNetUsers_UserID",
                table: "ProductReview",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_Product_ProductID",
                table: "ProductReview",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecification_AspNetUsers_CreatedById",
                table: "ProductSpecification",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecification_AspNetUsers_ModifiedById",
                table: "ProductSpecification",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSpecification_Product_ProductID",
                table: "ProductSpecification",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
