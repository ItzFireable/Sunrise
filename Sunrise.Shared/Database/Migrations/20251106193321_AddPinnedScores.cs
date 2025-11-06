using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sunrise.Shared.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddPinnedScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_score_BeatmapId_IsScoreable_IsPassed_SubmissionStatus",
                table: "score");

            migrationBuilder.AddColumn<bool>(
                name: "IsPinned",
                table: "score",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_score_BeatmapId_IsScoreable_IsPassed_IsPinned_SubmissionStat~",
                table: "score",
                columns: new[] { "BeatmapId", "IsScoreable", "IsPassed", "IsPinned", "SubmissionStatus" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_score_BeatmapId_IsScoreable_IsPassed_IsPinned_SubmissionStat~",
                table: "score");

            migrationBuilder.DropColumn(
                name: "IsPinned",
                table: "score");

            migrationBuilder.CreateIndex(
                name: "IX_score_BeatmapId_IsScoreable_IsPassed_SubmissionStatus",
                table: "score",
                columns: new[] { "BeatmapId", "IsScoreable", "IsPassed", "SubmissionStatus" });
        }
    }
}
