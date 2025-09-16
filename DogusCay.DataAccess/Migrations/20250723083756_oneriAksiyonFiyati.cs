using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class oneriAksiyonFiyati : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AksiyonSatisFiyati",
                table: "TalepForms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OneriAksiyonFiyati",
                table: "TalepForms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OneriRafFiyati",
                table: "TalepForms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OneriAksiyonFiyati",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OneriRafFiyati",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistributorLogoKod",
                table: "Distributors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AksiyonSatisFiyati",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "OneriAksiyonFiyati",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "OneriRafFiyati",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "OneriAksiyonFiyati",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OneriRafFiyati",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DistributorLogoKod",
                table: "Distributors");
        }
    }
}
