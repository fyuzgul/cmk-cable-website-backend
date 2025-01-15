using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CmkCable.DataAccess.Migrations
{
    public partial class historyitemtranslationsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CareerForms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "HistoryItems");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "HistoryItems");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "HistoryItems");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "HistoryItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HistoryItemTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(nullable: false),
                    HistoryItemId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryItemTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryItemTranslations_HistoryItems_HistoryItemId",
                        column: x => x.HistoryItemId,
                        principalTable: "HistoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoryItemTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoryItemTranslations_HistoryItemId",
                table: "HistoryItemTranslations",
                column: "HistoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryItemTranslations_LanguageId",
                table: "HistoryItemTranslations",
                column: "LanguageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryItemTranslations");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "HistoryItems");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "HistoryItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "HistoryItems",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "HistoryItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CareerForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CVData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Consent = table.Column<bool>(type: "bit", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverLicense = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Faculty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GraduationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Languages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MilitaryStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seminars = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoftwareSkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelAvailability = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    University = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerForms", x => x.Id);
                });
        }
    }
}
