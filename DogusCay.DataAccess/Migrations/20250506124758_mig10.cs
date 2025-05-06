using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Kanals_KanalId",
                table: "Points");

            migrationBuilder.AlterColumn<int>(
                name: "KanalId",
                table: "Points",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Kanals_KanalId",
                table: "Points",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Kanals_KanalId",
                table: "Points");

            migrationBuilder.AlterColumn<int>(
                name: "KanalId",
                table: "Points",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Kanals_KanalId",
                table: "Points",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId");
        }
    }
}
