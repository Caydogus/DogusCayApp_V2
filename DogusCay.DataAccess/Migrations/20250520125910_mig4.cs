using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Kanals_KanalId",
                table: "Distributors");

            migrationBuilder.AlterColumn<int>(
                name: "KanalId",
                table: "Distributors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Distributors_Kanals_KanalId",
                table: "Distributors",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Kanals_KanalId",
                table: "Distributors");

            migrationBuilder.AlterColumn<int>(
                name: "KanalId",
                table: "Distributors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Distributors_Kanals_KanalId",
                table: "Distributors",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
