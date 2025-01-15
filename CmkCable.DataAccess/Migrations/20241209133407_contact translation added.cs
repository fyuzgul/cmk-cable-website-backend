using Microsoft.EntityFrameworkCore.Migrations;

namespace CmkCable.DataAccess.Migrations
{
    public partial class contacttranslationadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "ContactInformations");

            migrationBuilder.CreateTable(
                name: "ContactInformationTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactInformationId = table.Column<int>(nullable: false),
                    LanguageId = table.Column<int>(nullable: false),
                    Department = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInformationTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInformationTranslations_ContactInformations_ContactInformationId",
                        column: x => x.ContactInformationId,
                        principalTable: "ContactInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactInformationTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformationTranslations_ContactInformationId",
                table: "ContactInformationTranslations",
                column: "ContactInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformationTranslations_LanguageId",
                table: "ContactInformationTranslations",
                column: "LanguageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactInformationTranslations");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "ContactInformations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
