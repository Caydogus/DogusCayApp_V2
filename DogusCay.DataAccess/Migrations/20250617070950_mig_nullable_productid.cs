using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_nullable_productid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_Products_ProductId",
                table: "TalepForms");

            migrationBuilder.DropTable(
                name: "TalepFormItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "TalepForms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "TalepForms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TalepFormItems",
                columns: table => new
                {
                    TalepFormItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    TalepFormId = table.Column<int>(type: "int", nullable: false),
                    AdetFarkDonusuTL = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApproximateWeightKg = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BrutTotal = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ErpCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Iskonto1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Iskonto2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Iskonto3 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Iskonto4 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    KoliIciAdet = table.Column<int>(type: "int", nullable: false),
                    KoliIciToplamAdet = table.Column<int>(type: "int", nullable: false),
                    KoliToplamAgirligiKg = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ListeFiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SabitBedelTL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SonAdetFiyati = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalepFormItems", x => x.TalepFormItemId);
                    table.ForeignKey(
                        name: "FK_TalepFormItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TalepFormItems_Categories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TalepFormItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TalepFormItems_TalepForms_TalepFormId",
                        column: x => x.TalepFormId,
                        principalTable: "TalepForms",
                        principalColumn: "TalepFormId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TalepFormItems_CategoryId",
                table: "TalepFormItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TalepFormItems_ProductId",
                table: "TalepFormItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TalepFormItems_SubCategoryId",
                table: "TalepFormItems",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TalepFormItems_TalepFormId",
                table: "TalepFormItems",
                column: "TalepFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_Products_ProductId",
                table: "TalepForms",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
