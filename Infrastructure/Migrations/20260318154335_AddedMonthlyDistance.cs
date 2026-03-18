using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMonthlyDistance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_Journeys_JourneyId",
                table: "Audits");

            migrationBuilder.DropIndex(
                name: "IX_Audits_JourneyId",
                table: "Audits");

            migrationBuilder.DropColumn(
                name: "JourneyId",
                table: "Audits");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MonthlyDistances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    TotalDistanceKm = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyDistances", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyDistances_UserId_Year_Month",
                table: "MonthlyDistances",
                columns: new[] { "UserId", "Year", "Month" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyDistances");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "JourneyId",
                table: "Audits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Audits_JourneyId",
                table: "Audits",
                column: "JourneyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_Journeys_JourneyId",
                table: "Audits",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
