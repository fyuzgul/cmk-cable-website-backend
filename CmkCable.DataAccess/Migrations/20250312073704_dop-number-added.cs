using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmkCable.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class dopnumberadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DopNumber",
                table: "Certificates",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DopNumber",
                table: "Certificates");
        }
    }
}
