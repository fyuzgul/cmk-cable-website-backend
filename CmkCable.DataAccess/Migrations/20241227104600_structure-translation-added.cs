using Microsoft.EntityFrameworkCore.Migrations;

namespace CmkCable.DataAccess.Migrations
{
    public partial class structuretranslationadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Structures_Languages_LanguageId",
                table: "Structures");

            migrationBuilder.DropForeignKey(
                name: "FK_Structures_Products_ProductId",
                table: "Structures");

            migrationBuilder.DropIndex(
                name: "IX_Structures_LanguageId",
                table: "Structures");

            migrationBuilder.DropIndex(
                name: "IX_Structures_ProductId",
                table: "Structures");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Structures");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Structures");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Structures");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Structures",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StructureTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(nullable: false),
                    StructureId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StructureTranslations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StructureTranslations");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Structures");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Structures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Structures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Structures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Structures_LanguageId",
                table: "Structures",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Structures_ProductId",
                table: "Structures",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Structures_Languages_LanguageId",
                table: "Structures",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Structures_Products_ProductId",
                table: "Structures",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
