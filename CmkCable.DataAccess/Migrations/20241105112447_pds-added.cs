using Microsoft.EntityFrameworkCore.Migrations;

namespace CmkCable.DataAccess.Migrations
{
    public partial class pdsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PdsDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileContent = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdsDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PdsDocumentsTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    PdsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdsDocumentsTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PdsDocumentsTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PdsDocumentsTranslations_PdsDocuments_PdsId",
                        column: x => x.PdsId,
                        principalTable: "PdsDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PdsDocumentsTranslations_LanguageId",
                table: "PdsDocumentsTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PdsDocumentsTranslations_PdsId",
                table: "PdsDocumentsTranslations",
                column: "PdsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PdsDocumentsTranslations");

            migrationBuilder.DropTable(
                name: "PdsDocuments");
        }
    }
}
