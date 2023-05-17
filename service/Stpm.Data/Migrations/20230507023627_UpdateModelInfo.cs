using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stpm.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Topic",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_TopicId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "ProjectTimeline");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Post");

            migrationBuilder.AddColumn<string>(
                name: "GradeName",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelDate",
                table: "Topic",
                type: "smalldatetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Topic",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeaderId",
                table: "Topic",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Registered",
                table: "Topic",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Topic",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Passed",
                table: "SpecificAward",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowOn",
                table: "ProjectTimeline",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IssuedAt = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topic_LeaderId",
                table: "Topic",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Users",
                table: "Topic",
                column: "LeaderId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Users",
                table: "Topic");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_Topic_LeaderId",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "GradeName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CancelDate",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "LeaderId",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "Registered",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "Passed",
                table: "SpecificAward");

            migrationBuilder.DropColumn(
                name: "ShowOn",
                table: "ProjectTimeline");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "ProjectTimeline",
                type: "smalldatetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Post_TopicId",
                table: "Post",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Topic",
                table: "Post",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
