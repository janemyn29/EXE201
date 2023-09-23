using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResponsibilityList_Customers_CustomerId",
                table: "ResponsibilityList");

            migrationBuilder.DropForeignKey(
                name: "FK_ResponsibilityList_Managers_ManagerId",
                table: "ResponsibilityList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResponsibilityList",
                table: "ResponsibilityList");

            migrationBuilder.RenameTable(
                name: "ResponsibilityList",
                newName: "ResponsibilityLists");

            migrationBuilder.RenameIndex(
                name: "IX_ResponsibilityList_ManagerId",
                table: "ResponsibilityLists",
                newName: "IX_ResponsibilityLists_ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_ResponsibilityList_CustomerId",
                table: "ResponsibilityLists",
                newName: "IX_ResponsibilityLists_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResponsibilityLists",
                table: "ResponsibilityLists",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicePaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_ServicePayment_ServicePaymentId",
                        column: x => x.ServicePaymentId,
                        principalTable: "ServicePayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ServicePaymentId",
                table: "Transactions",
                column: "ServicePaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResponsibilityLists_Customers_CustomerId",
                table: "ResponsibilityLists",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResponsibilityLists_Managers_ManagerId",
                table: "ResponsibilityLists",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResponsibilityLists_Customers_CustomerId",
                table: "ResponsibilityLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ResponsibilityLists_Managers_ManagerId",
                table: "ResponsibilityLists");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResponsibilityLists",
                table: "ResponsibilityLists");

            migrationBuilder.RenameTable(
                name: "ResponsibilityLists",
                newName: "ResponsibilityList");

            migrationBuilder.RenameIndex(
                name: "IX_ResponsibilityLists_ManagerId",
                table: "ResponsibilityList",
                newName: "IX_ResponsibilityList_ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_ResponsibilityLists_CustomerId",
                table: "ResponsibilityList",
                newName: "IX_ResponsibilityList_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResponsibilityList",
                table: "ResponsibilityList",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResponsibilityList_Customers_CustomerId",
                table: "ResponsibilityList",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResponsibilityList_Managers_ManagerId",
                table: "ResponsibilityList",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
