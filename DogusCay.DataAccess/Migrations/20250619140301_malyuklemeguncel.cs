using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class malyuklemeguncel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MalYuklemeTalepFormDetails_Products_ProductId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MalYuklemeTalepForms_Categories_CategoryId",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropForeignKey(
                name: "FK_MalYuklemeTalepForms_Categories_SubCategoryId",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropForeignKey(
                name: "FK_MalYuklemeTalepForms_Categories_SubSubCategoryId",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropIndex(
                name: "IX_MalYuklemeTalepForms_CategoryId",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropIndex(
                name: "IX_MalYuklemeTalepForms_SubCategoryId",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropIndex(
                name: "IX_MalYuklemeTalepFormDetails_ProductId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "BrutTotal",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "Iskonto1",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "Iskonto2",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "Iskonto3",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "Iskonto4",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "Maliyet",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "ValidFrom",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "MalYuklemeTalepForms");

            migrationBuilder.RenameColumn(
                name: "SubSubCategoryId",
                table: "MalYuklemeTalepForms",
                newName: "OnaylayanAdminId");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "MalYuklemeTalepForms",
                newName: "TalepTip");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "MalYuklemeTalepForms",
                newName: "TalepDurumu");

            migrationBuilder.RenameIndex(
                name: "IX_MalYuklemeTalepForms_SubSubCategoryId",
                table: "MalYuklemeTalepForms",
                newName: "IX_MalYuklemeTalepForms_OnaylayanAdminId");

            migrationBuilder.AddColumn<decimal>(
                name: "ApproximateWeightKg",
                table: "MalYuklemeTalepFormDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "MalYuklemeTalepFormDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ErpCode",
                table: "MalYuklemeTalepFormDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KoliIciAdet",
                table: "MalYuklemeTalepFormDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "MalYuklemeTalepFormDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "MalYuklemeTalepFormDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "MalYuklemeTalepFormDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubSubCategoryId",
                table: "MalYuklemeTalepFormDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitTypeId",
                table: "MalYuklemeTalepFormDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MalYuklemeTalepForms_AspNetUsers_OnaylayanAdminId",
                table: "MalYuklemeTalepForms",
                column: "OnaylayanAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MalYuklemeTalepForms_AspNetUsers_OnaylayanAdminId",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "ApproximateWeightKg",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "ErpCode",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "KoliIciAdet",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "SubSubCategoryId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "UnitTypeId",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.RenameColumn(
                name: "TalepTip",
                table: "MalYuklemeTalepForms",
                newName: "SubCategoryId");

            migrationBuilder.RenameColumn(
                name: "TalepDurumu",
                table: "MalYuklemeTalepForms",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "OnaylayanAdminId",
                table: "MalYuklemeTalepForms",
                newName: "SubSubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_MalYuklemeTalepForms_OnaylayanAdminId",
                table: "MalYuklemeTalepForms",
                newName: "IX_MalYuklemeTalepForms_SubSubCategoryId");

            migrationBuilder.AddColumn<decimal>(
                name: "BrutTotal",
                table: "MalYuklemeTalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto1",
                table: "MalYuklemeTalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto2",
                table: "MalYuklemeTalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto3",
                table: "MalYuklemeTalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto4",
                table: "MalYuklemeTalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Maliyet",
                table: "MalYuklemeTalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "MalYuklemeTalepForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "MalYuklemeTalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidFrom",
                table: "MalYuklemeTalepForms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidTo",
                table: "MalYuklemeTalepForms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepForms_CategoryId",
                table: "MalYuklemeTalepForms",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepForms_SubCategoryId",
                table: "MalYuklemeTalepForms",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepFormDetails_ProductId",
                table: "MalYuklemeTalepFormDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_MalYuklemeTalepFormDetails_Products_ProductId",
                table: "MalYuklemeTalepFormDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MalYuklemeTalepForms_Categories_CategoryId",
                table: "MalYuklemeTalepForms",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MalYuklemeTalepForms_Categories_SubCategoryId",
                table: "MalYuklemeTalepForms",
                column: "SubCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MalYuklemeTalepForms_Categories_SubSubCategoryId",
                table: "MalYuklemeTalepForms",
                column: "SubSubCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
