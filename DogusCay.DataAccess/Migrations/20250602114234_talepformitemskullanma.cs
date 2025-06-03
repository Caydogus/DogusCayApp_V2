using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class talepformitemskullanma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AdetFarkDonusuTL",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AdetFarkDonusuYuzde",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "KoliIciToplamAdet",
                table: "TalepForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "KoliToplamAgirligiKg",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ListeFiyat",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SonAdetFiyati",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ToplamFiyat",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidFrom",
                table: "TalepForms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidTo",
                table: "TalepForms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdetFarkDonusuTL",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "AdetFarkDonusuYuzde",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "KoliIciToplamAdet",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "KoliToplamAgirligiKg",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "ListeFiyat",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "SonAdetFiyati",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "ToplamFiyat",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "ValidFrom",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "TalepForms");
        }
    }
}
