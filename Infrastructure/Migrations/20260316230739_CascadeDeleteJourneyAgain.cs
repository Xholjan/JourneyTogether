using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteJourneyAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_Journeys_JourneyId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_Journeys_JourneyId",
                table: "Favourites");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicLinks_Journeys_JourneyId",
                table: "PublicLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Journeys_JourneyId",
                table: "Shares");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_Journeys_JourneyId",
                table: "Audits",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_Journeys_JourneyId",
                table: "Favourites",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicLinks_Journeys_JourneyId",
                table: "PublicLinks",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Journeys_JourneyId",
                table: "Shares",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audits_Journeys_JourneyId",
                table: "Audits");

            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_Journeys_JourneyId",
                table: "Favourites");

            migrationBuilder.DropForeignKey(
                name: "FK_PublicLinks_Journeys_JourneyId",
                table: "PublicLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Journeys_JourneyId",
                table: "Shares");

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_Journeys_JourneyId",
                table: "Audits",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_Journeys_JourneyId",
                table: "Favourites",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicLinks_Journeys_JourneyId",
                table: "PublicLinks",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Journeys_JourneyId",
                table: "Shares",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
