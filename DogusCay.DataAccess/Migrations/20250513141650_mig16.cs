using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "TalepForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto1",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto2",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto3",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Iskonto4",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "KoliAgirligiKg",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "KoliFiyati",
                table: "TalepFormItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "KoliIciAdet",
                table: "TalepFormItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TalepForms_KanalId",
                table: "TalepForms",
                column: "KanalId");

            migrationBuilder.CreateIndex(
                name: "IX_TalepForms_PointGroupId",
                table: "TalepForms",
                column: "PointGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TalepForms_PointId",
                table: "TalepForms",
                column: "PointId");

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_Kanals_KanalId",
                table: "TalepForms",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_PointGroups_PointGroupId",
                table: "TalepForms",
                column: "PointGroupId",
                principalTable: "PointGroups",
                principalColumn: "PointGroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_Points_PointId",
                table: "TalepForms",
                column: "PointId",
                principalTable: "Points",
                principalColumn: "PointId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_Kanals_KanalId",
                table: "TalepForms");

            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_PointGroups_PointGroupId",
                table: "TalepForms");

            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_Points_PointId",
                table: "TalepForms");

            migrationBuilder.DropIndex(
                name: "IX_TalepForms_KanalId",
                table: "TalepForms");

            migrationBuilder.DropIndex(
                name: "IX_TalepForms_PointGroupId",
                table: "TalepForms");

            migrationBuilder.DropIndex(
                name: "IX_TalepForms_PointId",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "TalepForms");

            migrationBuilder.DropColumn(
                name: "Iskonto1",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "Iskonto2",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "Iskonto3",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "Iskonto4",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "KoliAgirligiKg",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "KoliFiyati",
                table: "TalepFormItems");

            migrationBuilder.DropColumn(
                name: "KoliIciAdet",
                table: "TalepFormItems");
        }
    }
}
