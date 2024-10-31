using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fall2024_Assignment3_mtorres3.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorTweet_Actor_ActorID",
                table: "ActorTweet");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieReview_Movie_MovieID",
                table: "MovieReview");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorTweet_Actor_ActorID",
                table: "ActorTweet",
                column: "ActorID",
                principalTable: "Actor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieReview_Movie_MovieID",
                table: "MovieReview",
                column: "MovieID",
                principalTable: "Movie",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorTweet_Actor_ActorID",
                table: "ActorTweet");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieReview_Movie_MovieID",
                table: "MovieReview");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorTweet_Actor_ActorID",
                table: "ActorTweet",
                column: "ActorID",
                principalTable: "Actor",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieReview_Movie_MovieID",
                table: "MovieReview",
                column: "MovieID",
                principalTable: "Movie",
                principalColumn: "ID");
        }
    }
}
