using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyncService.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNotMappedAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_NearEarthObjects_CloseApproachDataId",
                table: "NearEarthObjects",
                column: "CloseApproachDataId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NearEarthObjects_CloseApproachData_CloseApproachDataId",
                table: "NearEarthObjects",
                column: "CloseApproachDataId",
                principalTable: "CloseApproachData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NearEarthObjects_CloseApproachData_CloseApproachDataId",
                table: "NearEarthObjects");

            migrationBuilder.DropIndex(
                name: "IX_NearEarthObjects_CloseApproachDataId",
                table: "NearEarthObjects");
        }
    }
}
