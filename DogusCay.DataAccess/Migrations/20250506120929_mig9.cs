using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PointGroupId",
                table: "Points",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "KanalId",
                table: "Points",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Points_KanalId",
                table: "Points",
                column: "KanalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Kanals_KanalId",
                table: "Points",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Kanals_KanalId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Points_KanalId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "KanalId",
                table: "Points");

            migrationBuilder.AlterColumn<int>(
                name: "PointGroupId",
                table: "Points",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
