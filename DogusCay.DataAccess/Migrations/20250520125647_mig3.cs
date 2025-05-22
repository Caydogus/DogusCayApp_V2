using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KanalId",
                table: "Distributors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_KanalId",
                table: "Distributors",
                column: "KanalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Distributors_Kanals_KanalId",
                table: "Distributors",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Kanals_KanalId",
                table: "Distributors");

            migrationBuilder.DropIndex(
                name: "IX_Distributors_KanalId",
                table: "Distributors");

            migrationBuilder.DropColumn(
                name: "KanalId",
                table: "Distributors");
        }
    }
}
