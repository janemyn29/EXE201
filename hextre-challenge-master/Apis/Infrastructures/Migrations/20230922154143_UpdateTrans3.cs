using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrans3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_AspNetUsers_CustomerId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_AspNetUsers_CustomerId",
                table: "OrderDetail");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "OrderDetail",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "OrderDetail",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                table: "Feedback",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
                name: "FK_Feedback_Customers_CustomerId",
                table: "Feedback",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_AspNetUsers_ApplicationUserId",
                table: "OrderDetail",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Customers_CustomerId",
                table: "OrderDetail",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_AspNetUsers_ApplicationUserId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Customers_CustomerId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_AspNetUsers_ApplicationUserId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Customers_CustomerId",
                table: "OrderDetail");

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

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "OrderDetail",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "Feedback",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_AspNetUsers_CustomerId",
                table: "Feedback",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_AspNetUsers_CustomerId",
                table: "OrderDetail",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
