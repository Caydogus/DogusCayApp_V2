using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migbruttotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BrutTotal",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TalepForms_CategoryId",
                table: "TalepForms",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TalepForms_SubCategoryId",
                table: "TalepForms",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TalepForms_SubSubCategoryId",
                table: "TalepForms",
                column: "SubSubCategoryId");

          

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_Categories_SubCategoryId",
                table: "TalepForms",
                column: "SubCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_Categories_SubSubCategoryId",
                table: "TalepForms",
                column: "SubSubCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_Categories_SubCategoryId",
                table: "TalepForms");

            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_Categories_SubSubCategoryId",
                table: "TalepForms");

            migrationBuilder.DropIndex(
                name: "IX_TalepForms_CategoryId",
                table: "TalepForms");

            migrationBuilder.DropIndex(
                name: "IX_TalepForms_SubCategoryId",
                table: "TalepForms");

            migrationBuilder.DropIndex(
                name: "IX_TalepForms_SubSubCategoryId",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "BrutTotal",
                table: "TalepForms");
        }
    }
}
