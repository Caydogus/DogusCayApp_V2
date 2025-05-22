using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Kanals_KanalId",
                table: "Distributors");

            migrationBuilder.AddForeignKey(
                name: "FK_Distributors_Kanals_KanalId",
                table: "Distributors",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Kanals_KanalId",
                table: "Distributors");

            migrationBuilder.AddForeignKey(
                name: "FK_Distributors_Kanals_KanalId",
                table: "Distributors",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId");
        }
    }
}
