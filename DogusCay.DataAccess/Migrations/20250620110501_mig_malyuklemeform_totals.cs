using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_malyuklemeform_totals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BrutTotal",
                table: "MalYuklemeTalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Maliyet",
                table: "MalYuklemeTalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ToplamAgirlikKg",
                table: "MalYuklemeTalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "MalYuklemeTalepForms",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrutTotal",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "Maliyet",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "ToplamAgirlikKg",
                table: "MalYuklemeTalepForms");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "MalYuklemeTalepForms");
        }
    }
}
