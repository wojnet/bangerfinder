using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bangerback.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    song_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    spotify_id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    artist = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    album = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Songs__A535AE1CCE8D58AD", x => x.song_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    is_verified = table.Column<bool>(type: "bit", nullable: false),
                    verification_token = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__B9BE370F0AD01AAF", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    song_id = table.Column<int>(type: "int", nullable: false),
                    score = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    generated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recommendation", x => new { x.user_id, x.song_id });
                    table.ForeignKey(
                        name: "FK__Recommend__song___4AB81AF0",
                        column: x => x.song_id,
                        principalTable: "Songs",
                        principalColumn: "song_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Recommend__user___49C3F6B7",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    session_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    token = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sessions__69B13FDC7CDBD6DB", x => x.session_id);
                    table.ForeignKey(
                        name: "FK__Sessions__user_i__3E52440B",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSongFavorites",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    song_id = table.Column<int>(type: "int", nullable: false),
                    added_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_song_favorite", x => new { x.user_id, x.song_id });
                    table.ForeignKey(
                        name: "FK__UserSongF__song___45F365D3",
                        column: x => x.song_id,
                        principalTable: "Songs",
                        principalColumn: "song_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__UserSongF__user___44FF419A",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_song_id",
                table: "Recommendations",
                column: "song_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_user_id",
                table: "Sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Sessions__CA90DA7A01109920",
                table: "Sessions",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Songs__C253CFF10D62E3EF",
                table: "Songs",
                column: "spotify_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Users__AB6E616455510AF1",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Users__F3DBC57274E78E26",
                table: "Users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSongFavorites_song_id",
                table: "UserSongFavorites",
                column: "song_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recommendations");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "UserSongFavorites");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
