using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TalepForms",
                columns: table => new
                {
                    TalepFormId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    TalepTip = table.Column<int>(type: "int", nullable: false),
                    KanalId = table.Column<int>(type: "int", nullable: false),
                    PointGroupId = table.Column<int>(type: "int", nullable: false),
                    PointId = table.Column<int>(type: "int", nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TalepDurumu = table.Column<int>(type: "int", nullable: false),
                    OnaylayanAdminId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalepForms", x => x.TalepFormId);
                    table.ForeignKey(
                        name: "FK_TalepForms_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TalepForms_AspNetUsers_OnaylayanAdminId",
                        column: x => x.OnaylayanAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TalepFormItems",
                columns: table => new
                {
                    TalepFormItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TalepFormId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalepFormItems", x => x.TalepFormItemId);
                    table.ForeignKey(
                        name: "FK_TalepFormItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TalepFormItems_TalepForms_TalepFormId",
                        column: x => x.TalepFormId,
                        principalTable: "TalepForms",
                        principalColumn: "TalepFormId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TalepFormItems_ProductId",
                table: "TalepFormItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TalepFormItems_TalepFormId",
                table: "TalepFormItems",
                column: "TalepFormId");

            migrationBuilder.CreateIndex(
                name: "IX_TalepForms_AppUserId",
                table: "TalepForms",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TalepForms_OnaylayanAdminId",
                table: "TalepForms",
                column: "OnaylayanAdminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TalepFormItems");

            migrationBuilder.DropTable(
                name: "TalepForms");
        }
    }
}
