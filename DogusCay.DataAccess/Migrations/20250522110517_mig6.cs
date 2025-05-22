using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // FK kaldır
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Regions_RegionId",
                table: "AspNetUsers");

            // Index kaldır
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RegionId",
                table: "AspNetUsers");

            // Region tablosunu kaldır
            migrationBuilder.DropTable(
                name: "Regions");

            // Kolonu kaldır (önce index kaldırıldığından hata vermez)
            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "AspNetUsers");

            // Yeni kolon ekle
            migrationBuilder.AddColumn<string>(
                name: "PointErc",
                table: "Points",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eklenen kolonu geri al
            migrationBuilder.DropColumn(
                name: "PointErc",
                table: "Points");
        }
    }
}
