using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmkCable.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RecreateGetOfferTableWithNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetOffers_CompanyTypes_CompanyTypeId",
                table: "GetOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_GetOffers_HelpTypes_HelpTypeId",
                table: "GetOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_GetOffers_Roles_RoleId",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "Ambalajlama",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "FirmaAdi",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "Kablolar",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "Lme",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "OdemeSekli",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "ParaBirimleri",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "Telefon",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "TeslimSekli",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "TeslimYeri",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "Ulke",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "Unvan",
                table: "GetOffers");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "GetOffers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HelpTypeId",
                table: "GetOffers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyTypeId",
                table: "GetOffers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "GetOffers",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "GetOffers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "GetOffers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "GetOffers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "GetOffers",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TelephoneNumber",
                table: "GetOffers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkEmail",
                table: "GetOffers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_GetOffers_CompanyTypes_CompanyTypeId",
                table: "GetOffers",
                column: "CompanyTypeId",
                principalTable: "CompanyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GetOffers_HelpTypes_HelpTypeId",
                table: "GetOffers",
                column: "HelpTypeId",
                principalTable: "HelpTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GetOffers_Roles_RoleId",
                table: "GetOffers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetOffers_CompanyTypes_CompanyTypeId",
                table: "GetOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_GetOffers_HelpTypes_HelpTypeId",
                table: "GetOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_GetOffers_Roles_RoleId",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "TelephoneNumber",
                table: "GetOffers");

            migrationBuilder.DropColumn(
                name: "WorkEmail",
                table: "GetOffers");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "GetOffers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "HelpTypeId",
                table: "GetOffers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyTypeId",
                table: "GetOffers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdSoyad",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ambalajlama",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirmaAdi",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kablolar",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lme",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OdemeSekli",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "ParaBirimleri",
                table: "GetOffers",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefon",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeslimSekli",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeslimYeri",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ulke",
                table: "GetOffers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unvan",
                table: "GetOffers",
                type: "text",
                nullable: true);

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
    }
}
