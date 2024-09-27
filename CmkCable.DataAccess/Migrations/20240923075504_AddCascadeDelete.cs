using Microsoft.EntityFrameworkCore.Migrations;

namespace CmkCable.DataAccess.Migrations
{
    public partial class AddCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Certificates_TypeId",
                table: "Certificates",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_CertificateTypes_TypeId",
                table: "Certificates",
                column: "TypeId",
                principalTable: "CertificateTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_CertificateTypes_TypeId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_TypeId",
                table: "Certificates");
        }
    }
}
