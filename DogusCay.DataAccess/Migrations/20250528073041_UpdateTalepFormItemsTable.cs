using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTalepFormItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KoliAgirligiKg",
                table: "TalepFormItems");

            migrationBuilder.RenameColumn(
                name: "KoliFiyati",
                table: "TalepFormItems",
                newName: "ApproximateWeightKg");

            migrationBuilder.AddColumn<decimal>(
                name: "ApproximateWeightKg",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto1",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto2",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto3",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto4",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KoliIciAdet",
                table: "TalepForms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "TalepForms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TalepForms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidFrom",
                table: "TalepForms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidTo",
                table: "TalepForms",
                type: "datetime2",
                nullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "TalepFormItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApproximateWeightKg",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TalepForms_ProductId",
                table: "TalepForms",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_Products_ProductId",
                table: "TalepForms",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_Products_ProductId",
                table: "TalepForms");

            migrationBuilder.DropIndex(
                name: "IX_TalepForms_ProductId",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "ApproximateWeightKg",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "Iskonto1",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "Iskonto2",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "Iskonto3",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "Iskonto4",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "KoliIciAdet",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "ValidFrom",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "TalepForms");

            migrationBuilder.RenameColumn(
                name: "ApproximateWeightKg",
                table: "TalepFormItems",
                newName: "KoliFiyati");

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

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "TalepFormItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "KoliAgirligiKg",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApproximateWeightKg",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
