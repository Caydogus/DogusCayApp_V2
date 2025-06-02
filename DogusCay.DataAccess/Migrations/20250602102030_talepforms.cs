using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class talepforms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SabitBedelTL",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdetFarkDonusuTL",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "KoliIciToplamAdet",
                table: "TalepFormItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "KoliToplamAgirligiKg",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ListeFiyat",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SabitBedelTL",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SonAdetFiyati",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SabitBedelTL",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "AdetFarkDonusuTL",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "KoliIciToplamAdet",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "KoliToplamAgirligiKg",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "ListeFiyat",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "SabitBedelTL",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "SonAdetFiyati",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "TalepFormItems");
        }
    }
}
