using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointGroups_Kanals_KanalId",
                table: "PointGroups");

            migrationBuilder.AlterColumn<int>(
                name: "KanalId",
                table: "PointGroups",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DistributorId",
                table: "PointGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Distributors",
                columns: table => new
                {
                    DistributorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistributorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DistributorErcKod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributors", x => x.DistributorId);
                    table.ForeignKey(
                        name: "FK_Distributors_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointGroups_DistributorId",
                table: "PointGroups",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_AppUserId",
                table: "Distributors",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointGroups_Distributors_DistributorId",
                table: "PointGroups",
                column: "DistributorId",
                principalTable: "Distributors",
                principalColumn: "DistributorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PointGroups_Kanals_KanalId",
                table: "PointGroups",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointGroups_Distributors_DistributorId",
                table: "PointGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_PointGroups_Kanals_KanalId",
                table: "PointGroups");

            migrationBuilder.DropTable(
                name: "Distributors");

            migrationBuilder.DropIndex(
                name: "IX_PointGroups_DistributorId",
                table: "PointGroups");

            migrationBuilder.DropColumn(
                name: "DistributorId",
                table: "PointGroups");

            migrationBuilder.AlterColumn<int>(
                name: "KanalId",
                table: "PointGroups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PointGroups_Kanals_KanalId",
                table: "PointGroups",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
