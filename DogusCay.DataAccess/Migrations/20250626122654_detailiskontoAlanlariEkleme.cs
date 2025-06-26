using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class detailiskontoAlanlariEkleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BrutTutar",
                table: "MalYuklemeTalepFormDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount1",
                table: "MalYuklemeTalepFormDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount2",
                table: "MalYuklemeTalepFormDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FixedPrice",
                table: "MalYuklemeTalepFormDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Maliyet",
                table: "MalYuklemeTalepFormDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NetAdetFiyat",
                table: "MalYuklemeTalepFormDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NetTutar",
                table: "MalYuklemeTalepFormDetails",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrutTutar",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "Discount1",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "Discount2",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "FixedPrice",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "Maliyet",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "NetAdetFiyat",
                table: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropColumn(
                name: "NetTutar",
                table: "MalYuklemeTalepFormDetails");
        }
    }
}
