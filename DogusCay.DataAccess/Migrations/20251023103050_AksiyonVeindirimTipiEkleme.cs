using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AksiyonVeindirimTipiEkleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AksiyonTipi",
                table: "TalepForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IndirimTipi",
                table: "TalepForms",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AksiyonTipi",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "IndirimTipi",
                table: "TalepForms");
        }
    }
}
