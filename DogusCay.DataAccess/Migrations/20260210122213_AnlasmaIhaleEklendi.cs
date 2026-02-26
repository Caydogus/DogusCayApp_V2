using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AnlasmaIhaleEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IhaleAnlasmaSozlesmeler",
                columns: table => new
                {
                    IhaleAnlasmaSozlesmeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoktaKod = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    IskontoOrani = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TalepDurumu = table.Column<int>(type: "int", nullable: false),
                    OnaylayanAdminId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IhaleAnlasmaSozlesmeler", x => x.IhaleAnlasmaSozlesmeId);
                    table.ForeignKey(
                        name: "FK_IhaleAnlasmaSozlesmeler_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IhaleAnlasmaSozlesmeler_AspNetUsers_OnaylayanAdminId",
                        column: x => x.OnaylayanAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IhaleAnlasmaDosyalar",
                columns: table => new
                {
                    IhaleAnlasmaDosyaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IhaleAnlasmaSozlesmeId = table.Column<int>(type: "int", nullable: false),
                    DosyaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosyaYolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosyaTipi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosyaBoyutu = table.Column<long>(type: "bigint", nullable: false),
                    SayfaSirasi = table.Column<int>(type: "int", nullable: false),
                    YuklenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IhaleAnlasmaDosyalar", x => x.IhaleAnlasmaDosyaId);
                    table.ForeignKey(
                        name: "FK_IhaleAnlasmaDosyalar_IhaleAnlasmaSozlesmeler_IhaleAnlasmaSozlesmeId",
                        column: x => x.IhaleAnlasmaSozlesmeId,
                        principalTable: "IhaleAnlasmaSozlesmeler",
                        principalColumn: "IhaleAnlasmaSozlesmeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IhaleAnlasmaDosyalar_IhaleAnlasmaSozlesmeId",
                table: "IhaleAnlasmaDosyalar",
                column: "IhaleAnlasmaSozlesmeId");

            migrationBuilder.CreateIndex(
                name: "IX_IhaleAnlasmaSozlesmeler_AppUserId",
                table: "IhaleAnlasmaSozlesmeler",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_IhaleAnlasmaSozlesmeler_NoktaKod",
                table: "IhaleAnlasmaSozlesmeler",
                column: "NoktaKod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IhaleAnlasmaSozlesmeler_OnaylayanAdminId",
                table: "IhaleAnlasmaSozlesmeler",
                column: "OnaylayanAdminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IhaleAnlasmaDosyalar");

            migrationBuilder.DropTable(
                name: "IhaleAnlasmaSozlesmeler");
        }
    }
}
