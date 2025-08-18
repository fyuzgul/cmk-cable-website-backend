using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmkCable.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGetOfferFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactRequests_CompanyTypes_CompanyTypeId",
                table: "ContactRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactRequests_HelpTypes_HelpTypeId",
                table: "ContactRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactRequests_Roles_RoleId",
                table: "ContactRequests");

            migrationBuilder.DropIndex(
                name: "IX_ContactRequests_CompanyTypeId",
                table: "ContactRequests");

            migrationBuilder.DropIndex(
                name: "IX_ContactRequests_HelpTypeId",
                table: "ContactRequests");

            migrationBuilder.DropColumn(
                name: "CompanyTypeId",
                table: "ContactRequests");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ContactRequests");

            migrationBuilder.DropColumn(
                name: "HelpTypeId",
                table: "ContactRequests");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ContactRequests");

            migrationBuilder.RenameColumn(
                name: "WorkEmail",
                table: "ContactRequests",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ContactRequests",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "Company",
                table: "ContactRequests",
                newName: "Email");

            migrationBuilder.AddColumn<int>(
                name: "CompanyTypeId",
                table: "GetOffers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HelpTypeId",
                table: "GetOffers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "GetOffers",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "ContactRequests",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ContactRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "ContactRequests",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "ContactRequests",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GetOffers_CompanyTypeId",
                table: "GetOffers",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GetOffers_HelpTypeId",
                table: "GetOffers",
                column: "HelpTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GetOffers_RoleId",
                table: "GetOffers",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactRequests_Roles_RoleId",
                table: "ContactRequests",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GetOffers_CompanyTypes_CompanyTypeId",
                table: "GetOffers",
                column: "CompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GetOffers_HelpTypes_HelpTypeId",
                table: "GetOffers",
                column: "HelpTypeId",
                principalTable: "HelpTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GetOffers_Roles_RoleId",
                table: "GetOffers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactRequests_Roles_RoleId",
                table: "ContactRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_GetOffers_CompanyTypes_CompanyTypeId",
                table: "GetOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_GetOffers_HelpTypes_HelpTypeId",
                table: "GetOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_GetOffers_Roles_RoleId",
                table: "GetOffers");

            migrationBuilder.DropIndex(
                name: "IX_GetOffers_CompanyTypeId",
                table: "GetOffers");

            migrationBuilder.DropIndex(
                name: "IX_GetOffers_HelpTypeId",
                table: "GetOffers");

            migrationBuilder.DropIndex(
                name: "IX_GetOffers_RoleId",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "CompanyTypeId",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "HelpTypeId",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "ContactRequests");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "ContactRequests");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "ContactRequests");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "ContactRequests",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "ContactRequests",
                newName: "WorkEmail");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "ContactRequests",
                newName: "Company");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "ContactRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyTypeId",
                table: "ContactRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ContactRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HelpTypeId",
                table: "ContactRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ContactRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ContactRequests_CompanyTypeId",
                table: "ContactRequests",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactRequests_HelpTypeId",
                table: "ContactRequests",
                column: "HelpTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactRequests_CompanyTypes_CompanyTypeId",
                table: "ContactRequests",
                column: "CompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactRequests_HelpTypes_HelpTypeId",
                table: "ContactRequests",
                column: "HelpTypeId",
                principalTable: "HelpTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactRequests_Roles_RoleId",
                table: "ContactRequests",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
