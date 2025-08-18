using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CmkCable.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExperienceAndEducationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "CareerInformations");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "CareerInformations");

            migrationBuilder.DropColumn(
                name: "GraduationDate",
                table: "CareerInformations");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "CareerInformations");

            migrationBuilder.DropColumn(
                name: "School",
                table: "CareerInformations");

            migrationBuilder.DropColumn(
                name: "Seminars",
                table: "CareerInformations");

            migrationBuilder.DropColumn(
                name: "SoftwareSkills",
                table: "CareerInformations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "CareerInformations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "CareerInformations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GraduationDate",
                table: "CareerInformations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Languages",
                table: "CareerInformations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School",
                table: "CareerInformations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seminars",
                table: "CareerInformations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoftwareSkills",
                table: "CareerInformations",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CareerInformationId = table.Column<int>(type: "integer", nullable: false),
                    Company = table.Column<string>(type: "text", nullable: true),
                    Duration = table.Column<string>(type: "text", nullable: true),
                    Position = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiences_CareerInformations_CareerInformationId",
                        column: x => x.CareerInformationId,
                        principalTable: "CareerInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_CareerInformationId",
                table: "Experiences",
                column: "CareerInformationId");
        }
    }
}
