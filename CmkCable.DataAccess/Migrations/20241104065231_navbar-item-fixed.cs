using Microsoft.EntityFrameworkCore.Migrations;

namespace CmkCable.DataAccess.Migrations
{
    public partial class navbaritemfixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "NavbarItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NavbarItems_ParentId",
                table: "NavbarItems",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_NavbarItems_NavbarItems_ParentId",
                table: "NavbarItems",
                column: "ParentId",
                principalTable: "NavbarItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NavbarItems_NavbarItems_ParentId",
                table: "NavbarItems");

            migrationBuilder.DropIndex(
                name: "IX_NavbarItems_ParentId",
                table: "NavbarItems");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "NavbarItems");
        }
    }
}
