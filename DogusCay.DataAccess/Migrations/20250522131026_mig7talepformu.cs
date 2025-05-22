using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig7talepformu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TalepFormItems_Categories_CategoryId",
                table: "TalepFormItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TalepFormItems_Categories_SubCategoryId",
                table: "TalepFormItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TalepFormItems_Products_ProductId",
                table: "TalepFormItems");

            migrationBuilder.RenameColumn(
                name: "TalepTarihi",
                table: "TalepForms",
                newName: "TalepBitisTarihi");

            migrationBuilder.AlterColumn<int>(
                name: "PointGroupTypeId",
                table: "TalepForms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DistributorId",
                table: "TalepForms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TalepBaslangicTarihi",
                table: "TalepForms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "Iskonto4",
                table: "TalepFormItems",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iskonto3",
                table: "TalepFormItems",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iskonto2",
                table: "TalepFormItems",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iskonto1",
                table: "TalepFormItems",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto1",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto2",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto3",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto4",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "KoliIciAdet",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TalepForms_DistributorId",
                table: "TalepForms",
                column: "DistributorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TalepFormItems_Categories_CategoryId",
                table: "TalepFormItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TalepFormItems_Categories_SubCategoryId",
                table: "TalepFormItems",
                column: "SubCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TalepFormItems_Products_ProductId",
                table: "TalepFormItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_Distributors_DistributorId",
                table: "TalepForms",
                column: "DistributorId",
                principalTable: "Distributors",
                principalColumn: "DistributorId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TalepFormItems_Categories_CategoryId",
                table: "TalepFormItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TalepFormItems_Categories_SubCategoryId",
                table: "TalepFormItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TalepFormItems_Products_ProductId",
                table: "TalepFormItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_Distributors_DistributorId",
                table: "TalepForms");

            migrationBuilder.DropIndex(
                name: "IX_TalepForms_DistributorId",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "DistributorId",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "TalepBaslangicTarihi",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "Iskonto1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Iskonto2",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Iskonto3",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Iskonto4",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "KoliIciAdet",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "TalepBitisTarihi",
                table: "TalepForms",
                newName: "TalepTarihi");

            migrationBuilder.AlterColumn<int>(
                name: "PointGroupTypeId",
                table: "TalepForms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Iskonto4",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iskonto3",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iskonto2",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Iskonto1",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_TalepFormItems_Categories_CategoryId",
                table: "TalepFormItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TalepFormItems_Categories_SubCategoryId",
                table: "TalepFormItems",
                column: "SubCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TalepFormItems_Products_ProductId",
                table: "TalepFormItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
