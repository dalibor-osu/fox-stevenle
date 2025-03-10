using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FoxStevenle.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "fox_stevenle");

            migrationBuilder.CreateTable(
                name: "daily_quizzes",
                schema: "fox_stevenle",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_daily_quizzes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "songs",
                schema: "fox_stevenle",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    cover_url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    duration = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_songs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "quiz_entries",
                schema: "fox_stevenle",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    song_number = table.Column<short>(type: "smallint", nullable: false),
                    song_id = table.Column<int>(type: "integer", nullable: false),
                    quiz_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quiz_entries", x => x.id);
                    table.ForeignKey(
                        name: "FK_quiz_entries_daily_quizzes_quiz_id",
                        column: x => x.quiz_id,
                        principalSchema: "fox_stevenle",
                        principalTable: "daily_quizzes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_quiz_entries_songs_song_id",
                        column: x => x.song_id,
                        principalSchema: "fox_stevenle",
                        principalTable: "songs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_daily_quizzes_date",
                schema: "fox_stevenle",
                table: "daily_quizzes",
                column: "date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_daily_quizzes_id",
                schema: "fox_stevenle",
                table: "daily_quizzes",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quiz_entries_id",
                schema: "fox_stevenle",
                table: "quiz_entries",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quiz_entries_quiz_id",
                schema: "fox_stevenle",
                table: "quiz_entries",
                column: "quiz_id");

            migrationBuilder.CreateIndex(
                name: "IX_quiz_entries_song_id",
                schema: "fox_stevenle",
                table: "quiz_entries",
                column: "song_id");

            migrationBuilder.CreateIndex(
                name: "IX_songs_id",
                schema: "fox_stevenle",
                table: "songs",
                column: "id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quiz_entries",
                schema: "fox_stevenle");

            migrationBuilder.DropTable(
                name: "daily_quizzes",
                schema: "fox_stevenle");

            migrationBuilder.DropTable(
                name: "songs",
                schema: "fox_stevenle");
        }
    }
}
