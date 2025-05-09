using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_SaleTypes_SaleTypeId",
                table: "Sales");

            migrationBuilder.AlterColumn<int>(
                name: "SaleTypeId",
                table: "Sales",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ManagerUserId",
                table: "Regions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ManagerUserId",
                table: "Regions",
                column: "ManagerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_AspNetUsers_ManagerUserId",
                table: "Regions",
                column: "ManagerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_SaleTypes_SaleTypeId",
                table: "Sales",
                column: "SaleTypeId",
                principalTable: "SaleTypes",
                principalColumn: "SaleTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Regions_AspNetUsers_ManagerUserId",
                table: "Regions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_SaleTypes_SaleTypeId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Regions_ManagerUserId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "ManagerUserId",
                table: "Regions");

            migrationBuilder.AlterColumn<int>(
                name: "SaleTypeId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_SaleTypes_SaleTypeId",
                table: "Sales",
                column: "SaleTypeId",
                principalTable: "SaleTypes",
                principalColumn: "SaleTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
