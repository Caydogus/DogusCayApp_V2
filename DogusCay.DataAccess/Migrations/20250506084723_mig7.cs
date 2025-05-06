using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_SalesPoints_SalesPointId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "SalesPoints");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Sales_SalesPointId",
                table: "Sales");

            migrationBuilder.AddColumn<int>(
                name: "PointId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Kanals",
                columns: table => new
                {
                    KanalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KanalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kanals", x => x.KanalId);
                });

            migrationBuilder.CreateTable(
                name: "PointGroups",
                columns: table => new
                {
                    PointGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KanalId = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointGroups", x => x.PointGroupId);
                    table.ForeignKey(
                        name: "FK_PointGroups_Kanals_KanalId",
                        column: x => x.KanalId,
                        principalTable: "Kanals",
                        principalColumn: "KanalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PointGroups_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId");
                });

            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    PointId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PointName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PointGroupId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.PointId);
                    table.ForeignKey(
                        name: "FK_Points_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Points_PointGroups_PointGroupId",
                        column: x => x.PointGroupId,
                        principalTable: "PointGroups",
                        principalColumn: "PointGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PointId",
                table: "Sales",
                column: "PointId");

            migrationBuilder.CreateIndex(
                name: "IX_PointGroups_KanalId",
                table: "PointGroups",
                column: "KanalId");

            migrationBuilder.CreateIndex(
                name: "IX_PointGroups_RegionId",
                table: "PointGroups",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_AppUserId",
                table: "Points",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Points_PointGroupId",
                table: "Points",
                column: "PointGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Points_PointId",
                table: "Sales",
                column: "PointId",
                principalTable: "Points",
                principalColumn: "PointId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Points_PointId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "PointGroups");

            migrationBuilder.DropTable(
                name: "Kanals");

            migrationBuilder.DropIndex(
                name: "IX_Sales_PointId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PointId",
                table: "Sales");

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    ChannelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.ChannelId);
                });

            migrationBuilder.CreateTable(
                name: "SalesPoints",
                columns: table => new
                {
                    SalesPointId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SalesPointName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesPoints", x => x.SalesPointId);
                    table.ForeignKey(
                        name: "FK_SalesPoints_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesPoints_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SalesPointId",
                table: "Sales",
                column: "SalesPointId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPoints_ChannelId",
                table: "SalesPoints",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPoints_RegionId",
                table: "SalesPoints",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_SalesPoints_SalesPointId",
                table: "Sales",
                column: "SalesPointId",
                principalTable: "SalesPoints",
                principalColumn: "SalesPointId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
