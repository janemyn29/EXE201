using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class fixContract2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "latitudeIP",
                table: "Warehouse",
                newName: "LatitudeIP");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplay",
                table: "Warehouse",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderDetailId",
                table: "Contract",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Contract",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisplay",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Contract");

            migrationBuilder.RenameColumn(
                name: "LatitudeIP",
                table: "Warehouse",
                newName: "latitudeIP");
        }
    }
}
