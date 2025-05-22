using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addpointgrouptpye : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Kanals_KanalId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_PointGroups_PointGroupId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_PointGroups_PointGroupId",
                table: "TalepForms");

            migrationBuilder.DropTable(
                name: "PointGroups");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "DistributorErcKod",
                table: "Distributors");

            migrationBuilder.RenameColumn(
                name: "PointGroupId",
                table: "TalepForms",
                newName: "PointGroupTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TalepForms_PointGroupId",
                table: "TalepForms",
                newName: "IX_TalepForms_PointGroupTypeId");

            migrationBuilder.RenameColumn(
                name: "PointGroupId",
                table: "Points",
                newName: "PointGroupTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Points_PointGroupId",
                table: "Points",
                newName: "IX_Points_PointGroupTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "PointName",
                table: "Points",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<int>(
                name: "KanalId",
                table: "Points",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DistributorId",
                table: "Points",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KanalName",
                table: "Kanals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "KanalId",
                table: "Distributors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DistributorName",
                table: "Distributors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "KanalId1",
                table: "Distributors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PointGroupTypes",
                columns: table => new
                {
                    PointGroupTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PointGroupTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointGroupTypes", x => x.PointGroupTypeId);
                    table.ForeignKey(
                        name: "FK_PointGroupTypes_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Points_DistributorId",
                table: "Points",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_KanalId1",
                table: "Distributors",
                column: "KanalId1");

            migrationBuilder.CreateIndex(
                name: "IX_PointGroupTypes_RegionId",
                table: "PointGroupTypes",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Distributors_Kanals_KanalId1",
                table: "Distributors",
                column: "KanalId1",
                principalTable: "Kanals",
                principalColumn: "KanalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Distributors_DistributorId",
                table: "Points",
                column: "DistributorId",
                principalTable: "Distributors",
                principalColumn: "DistributorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Kanals_KanalId",
                table: "Points",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_PointGroupTypes_PointGroupTypeId",
                table: "Points",
                column: "PointGroupTypeId",
                principalTable: "PointGroupTypes",
                principalColumn: "PointGroupTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_PointGroupTypes_PointGroupTypeId",
                table: "TalepForms",
                column: "PointGroupTypeId",
                principalTable: "PointGroupTypes",
                principalColumn: "PointGroupTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Kanals_KanalId1",
                table: "Distributors");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Distributors_DistributorId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_Kanals_KanalId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_Points_PointGroupTypes_PointGroupTypeId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_TalepForms_PointGroupTypes_PointGroupTypeId",
                table: "TalepForms");

            migrationBuilder.DropTable(
                name: "PointGroupTypes");

            migrationBuilder.DropIndex(
                name: "IX_Points_DistributorId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Distributors_KanalId1",
                table: "Distributors");

            migrationBuilder.DropColumn(
                name: "DistributorId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "KanalId1",
                table: "Distributors");

            migrationBuilder.RenameColumn(
                name: "PointGroupTypeId",
                table: "TalepForms",
                newName: "PointGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_TalepForms_PointGroupTypeId",
                table: "TalepForms",
                newName: "IX_TalepForms_PointGroupId");

            migrationBuilder.RenameColumn(
                name: "PointGroupTypeId",
                table: "Points",
                newName: "PointGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Points_PointGroupTypeId",
                table: "Points",
                newName: "IX_Points_PointGroupId");

            migrationBuilder.AlterColumn<string>(
                name: "PointName",
                table: "Points",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "KanalId",
                table: "Points",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Points",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Points",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "KanalName",
                table: "Kanals",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "KanalId",
                table: "Distributors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DistributorName",
                table: "Distributors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DistributorErcKod",
                table: "Distributors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PointGroups",
                columns: table => new
                {
                    PointGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistributorId = table.Column<int>(type: "int", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KanalId = table.Column<int>(type: "int", nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointGroups", x => x.PointGroupId);
                    table.ForeignKey(
                        name: "FK_PointGroups_Distributors_DistributorId",
                        column: x => x.DistributorId,
                        principalTable: "Distributors",
                        principalColumn: "DistributorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointGroups_Kanals_KanalId",
                        column: x => x.KanalId,
                        principalTable: "Kanals",
                        principalColumn: "KanalId");
                    table.ForeignKey(
                        name: "FK_PointGroups_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointGroups_DistributorId",
                table: "PointGroups",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_PointGroups_KanalId",
                table: "PointGroups",
                column: "KanalId");

            migrationBuilder.CreateIndex(
                name: "IX_PointGroups_RegionId",
                table: "PointGroups",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Kanals_KanalId",
                table: "Points",
                column: "KanalId",
                principalTable: "Kanals",
                principalColumn: "KanalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Points_PointGroups_PointGroupId",
                table: "Points",
                column: "PointGroupId",
                principalTable: "PointGroups",
                principalColumn: "PointGroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TalepForms_PointGroups_PointGroupId",
                table: "TalepForms",
                column: "PointGroupId",
                principalTable: "PointGroups",
                principalColumn: "PointGroupId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
