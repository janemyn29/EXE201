using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_AspNetUsers_ApplicationUserId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_AspNetUsers_ApplicationUserId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_AspNetUsers_CustomerId",
                table: "Request");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_ApplicationUserId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_ApplicationUserId",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Feedback");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Request",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Customers_CustomerId",
                table: "Request",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Customers_CustomerId",
                table: "Request");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "Request",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "OrderDetail",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Feedback",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ApplicationUserId",
                table: "OrderDetail",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ApplicationUserId",
                table: "Feedback",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_AspNetUsers_ApplicationUserId",
                table: "Feedback",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_AspNetUsers_ApplicationUserId",
                table: "OrderDetail",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_AspNetUsers_CustomerId",
                table: "Request",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
