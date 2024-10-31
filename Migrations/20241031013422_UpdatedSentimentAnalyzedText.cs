using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fall2024_Assignment3_mtorres3.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSentimentAnalyzedText : Migration
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

            migrationBuilder.DropColumn(
                name: "Sentiment",
                table: "MovieReview");

            migrationBuilder.DropColumn(
                name: "Sentiment",
                table: "ActorTweet");

            migrationBuilder.AlterColumn<int>(
                name: "MovieID",
                table: "MovieReview",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "Compound",
                table: "MovieReview",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Negative",
                table: "MovieReview",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Neutral",
                table: "MovieReview",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Positive",
                table: "MovieReview",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "ActorID",
                table: "ActorTweet",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "Compound",
                table: "ActorTweet",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Negative",
                table: "ActorTweet",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Neutral",
                table: "ActorTweet",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Positive",
                table: "ActorTweet",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorTweet_Actor_ActorID",
                table: "ActorTweet");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieReview_Movie_MovieID",
                table: "MovieReview");

            migrationBuilder.DropColumn(
                name: "Compound",
                table: "MovieReview");

            migrationBuilder.DropColumn(
                name: "Negative",
                table: "MovieReview");

            migrationBuilder.DropColumn(
                name: "Neutral",
                table: "MovieReview");

            migrationBuilder.DropColumn(
                name: "Positive",
                table: "MovieReview");

            migrationBuilder.DropColumn(
                name: "Compound",
                table: "ActorTweet");

            migrationBuilder.DropColumn(
                name: "Negative",
                table: "ActorTweet");

            migrationBuilder.DropColumn(
                name: "Neutral",
                table: "ActorTweet");

            migrationBuilder.DropColumn(
                name: "Positive",
                table: "ActorTweet");

            migrationBuilder.AlterColumn<int>(
                name: "MovieID",
                table: "MovieReview",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sentiment",
                table: "MovieReview",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ActorID",
                table: "ActorTweet",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sentiment",
                table: "ActorTweet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
