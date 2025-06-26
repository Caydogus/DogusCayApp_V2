using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_malyukleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MalYuklemeTalepForms",
                columns: table => new
                {
                    MalYuklemeTalepFormId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    KanalId = table.Column<int>(type: "int", nullable: false),
                    DistributorId = table.Column<int>(type: "int", nullable: true),
                    PointGroupTypeId = table.Column<int>(type: "int", nullable: true),
                    PointId = table.Column<int>(type: "int", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Iskonto1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Iskonto2 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Iskonto3 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Iskonto4 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BrutTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Maliyet = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    SubSubCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MalYuklemeTalepForms", x => x.MalYuklemeTalepFormId);
                    table.ForeignKey(
                        name: "FK_MalYuklemeTalepForms_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MalYuklemeTalepForms_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MalYuklemeTalepForms_Categories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MalYuklemeTalepForms_Categories_SubSubCategoryId",
                        column: x => x.SubSubCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MalYuklemeTalepForms_Distributors_DistributorId",
                        column: x => x.DistributorId,
                        principalTable: "Distributors",
                        principalColumn: "DistributorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MalYuklemeTalepForms_Kanals_KanalId",
                        column: x => x.KanalId,
                        principalTable: "Kanals",
                        principalColumn: "KanalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MalYuklemeTalepForms_PointGroupTypes_PointGroupTypeId",
                        column: x => x.PointGroupTypeId,
                        principalTable: "PointGroupTypes",
                        principalColumn: "PointGroupTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MalYuklemeTalepForms_Points_PointId",
                        column: x => x.PointId,
                        principalTable: "Points",
                        principalColumn: "PointId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MalYuklemeTalepFormDetails",
                columns: table => new
                {
                    MalYuklemeTalepFormDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MalYuklemeTalepFormId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MalYuklemeTalepFormDetails", x => x.MalYuklemeTalepFormDetailId);
                    table.ForeignKey(
                        name: "FK_MalYuklemeTalepFormDetails_MalYuklemeTalepForms_MalYuklemeTalepFormId",
                        column: x => x.MalYuklemeTalepFormId,
                        principalTable: "MalYuklemeTalepForms",
                        principalColumn: "MalYuklemeTalepFormId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MalYuklemeTalepFormDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepFormDetails_MalYuklemeTalepFormId",
                table: "MalYuklemeTalepFormDetails",
                column: "MalYuklemeTalepFormId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepFormDetails_ProductId",
                table: "MalYuklemeTalepFormDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepForms_AppUserId",
                table: "MalYuklemeTalepForms",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepForms_CategoryId",
                table: "MalYuklemeTalepForms",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepForms_DistributorId",
                table: "MalYuklemeTalepForms",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepForms_KanalId",
                table: "MalYuklemeTalepForms",
                column: "KanalId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepForms_PointGroupTypeId",
                table: "MalYuklemeTalepForms",
                column: "PointGroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepForms_PointId",
                table: "MalYuklemeTalepForms",
                column: "PointId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepForms_SubCategoryId",
                table: "MalYuklemeTalepForms",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MalYuklemeTalepForms_SubSubCategoryId",
                table: "MalYuklemeTalepForms",
                column: "SubSubCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MalYuklemeTalepFormDetails");

            migrationBuilder.DropTable(
                name: "MalYuklemeTalepForms");
        }
    }
}
