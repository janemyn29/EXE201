using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class fixContract4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Deposit",
                table: "Contract",
                newName: "DepositFee");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisplay",
                table: "Provider",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisplay",
                table: "Provider");

            migrationBuilder.RenameColumn(
                name: "DepositFee",
                table: "Contract",
                newName: "Deposit");
        }
    }
}
