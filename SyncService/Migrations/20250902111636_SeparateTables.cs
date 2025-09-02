using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyncService.Migrations
{
    /// <inheritdoc />
    public partial class SeparateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseApproachDate",
                table: "NearEarthObjects");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "NearEarthObjects");

            migrationBuilder.DropColumn(
                name: "MissDistanceKm",
                table: "NearEarthObjects");

            migrationBuilder.DropColumn(
                name: "RelativeVelocityKmh",
                table: "NearEarthObjects");

            migrationBuilder.AddColumn<Guid>(
                name: "CloseApproachDataId",
                table: "NearEarthObjects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SyncDateTimesId",
                table: "NearEarthObjects",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CloseApproachData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CloseApproachDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EpochDateCloseApproach = table.Column<long>(type: "bigint", nullable: false),
                    RelativeVelocityKmh = table.Column<double>(type: "double precision", nullable: false),
                    MissDistanceKm = table.Column<double>(type: "double precision", nullable: false),
                    OrbitingBody = table.Column<string>(type: "text", nullable: true),
                    NearEarthObjectId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CloseApproachData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyncDateTimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SyncTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncDateTimes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NearEarthObjects_SyncDateTimesId",
                table: "NearEarthObjects",
                column: "SyncDateTimesId");

            migrationBuilder.AddForeignKey(
                name: "FK_NearEarthObjects_SyncDateTimes_SyncDateTimesId",
                table: "NearEarthObjects",
                column: "SyncDateTimesId",
                principalTable: "SyncDateTimes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NearEarthObjects_SyncDateTimes_SyncDateTimesId",
                table: "NearEarthObjects");

            migrationBuilder.DropTable(
                name: "CloseApproachData");

            migrationBuilder.DropTable(
                name: "SyncDateTimes");

            migrationBuilder.DropIndex(
                name: "IX_NearEarthObjects_SyncDateTimesId",
                table: "NearEarthObjects");

            migrationBuilder.DropColumn(
                name: "CloseApproachDataId",
                table: "NearEarthObjects");

            migrationBuilder.DropColumn(
                name: "SyncDateTimesId",
                table: "NearEarthObjects");

            migrationBuilder.AddColumn<DateTime>(
                name: "CloseApproachDate",
                table: "NearEarthObjects",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "NearEarthObjects",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "MissDistanceKm",
                table: "NearEarthObjects",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RelativeVelocityKmh",
                table: "NearEarthObjects",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
