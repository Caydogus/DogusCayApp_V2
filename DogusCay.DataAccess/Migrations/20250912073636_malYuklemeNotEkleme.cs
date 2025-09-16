using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class malYuklemeNotEkleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "MalYuklemeTalepForms",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "MalYuklemeTalepForms");
        }
    }
}
