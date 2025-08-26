using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyncService.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NearEarthObjects",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CloseApproachDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstimatedDiameterMin = table.Column<double>(type: "double precision", nullable: false),
                    EstimatedDiameterMax = table.Column<double>(type: "double precision", nullable: false),
                    IsPotentiallyHazardous = table.Column<bool>(type: "boolean", nullable: false),
                    RelativeVelocityKmh = table.Column<double>(type: "double precision", nullable: false),
                    MissDistanceKm = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NearEarthObjects", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NearEarthObjects");
        }
    }
}
