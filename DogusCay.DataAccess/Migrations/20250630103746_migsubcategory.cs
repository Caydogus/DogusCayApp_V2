using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migsubcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_Distributors_DistributorId",
                table: "TalepForms");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Sales",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepFormDetails_CategoryId",
                table: "MalYuklemeTalepFormDetails",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepFormDetails_ProductId",
                table: "MalYuklemeTalepFormDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepFormDetails_SubCategoryId",
                table: "MalYuklemeTalepFormDetails",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepFormDetails_SubSubCategoryId",
                table: "MalYuklemeTalepFormDetails",
                column: "SubSubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MalYuklemeTalepFormDetails_Categories_CategoryId",
                table: "MalYuklemeTalepFormDetails",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MalYuklemeTalepFormDetails_Categories_SubCategoryId",
                table: "MalYuklemeTalepFormDetails",
                column: "SubCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MalYuklemeTalepFormDetails_Categories_SubSubCategoryId",
                table: "MalYuklemeTalepFormDetails",
                column: "SubSubCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MalYuklemeTalepFormDetails_Products_ProductId",
                table: "MalYuklemeTalepFormDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_Distributors_DistributorId",
                table: "TalepForms",
                column: "DistributorId",
                principalTable: "Distributors",
                principalColumn: "DistributorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MalYuklemeTalepFormDetails_Categories_CategoryId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MalYuklemeTalepFormDetails_Categories_SubCategoryId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MalYuklemeTalepFormDetails_Categories_SubSubCategoryId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MalYuklemeTalepFormDetails_Products_ProductId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_Distributors_DistributorId",
                table: "TalepForms");

            migrationBuilder.DropIndex(
                name: "IX_MalYuklemeTalepFormDetails_CategoryId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropIndex(
                name: "IX_MalYuklemeTalepFormDetails_ProductId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropIndex(
                name: "IX_MalYuklemeTalepFormDetails_SubCategoryId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropIndex(
                name: "IX_MalYuklemeTalepFormDetails_SubSubCategoryId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_Distributors_DistributorId",
                table: "TalepForms",
                column: "DistributorId",
                principalTable: "Distributors",
                principalColumn: "DistributorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
