using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class addBlog2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hashtag_Hashtag_HashtagId",
                table: "Hashtag");

            migrationBuilder.DropIndex(
                name: "IX_Hashtag_HashtagId",
                table: "Hashtag");

            migrationBuilder.DropColumn(
                name: "HashtagId",
                table: "Hashtag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HashtagId",
                table: "Hashtag",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hashtag_HashtagId",
                table: "Hashtag",
                column: "HashtagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hashtag_Hashtag_HashtagId",
                table: "Hashtag",
                column: "HashtagId",
                principalTable: "Hashtag",
                principalColumn: "Id");
        }
    }
}
