using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Managers_StaffId",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_StaffId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "Contract");

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                table: "Contract",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ManagerId",
                table: "Contract",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Managers_ManagerId",
                table: "Contract",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Managers_ManagerId",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_ManagerId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Contract");

            migrationBuilder.AddColumn<Guid>(
                name: "StaffId",
                table: "Contract",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contract_StaffId",
                table: "Contract",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Managers_StaffId",
                table: "Contract",
                column: "StaffId",
                principalTable: "Managers",
                principalColumn: "Id");
        }
    }
}
