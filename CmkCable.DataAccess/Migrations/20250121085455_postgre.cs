using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CmkCable.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class postgre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    FileContent = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificateTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    FaxNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInformations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomePageTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageCode = table.Column<string>(type: "text", nullable: true),
                    LanguageName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NavbarItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Route = table.Column<string>(type: "text", nullable: true),
                    ParentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavbarItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NavbarItems_NavbarItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "NavbarItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PdsDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileContent = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdsDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Standarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standarts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Structures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Structures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StructureTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    StructureId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StructureTranslations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SwiperItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwiperItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    MaxTemp = table.Column<double>(type: "double precision", nullable: false),
                    RatedVoltage = table.Column<double>(type: "double precision", nullable: false),
                    Section = table.Column<double>(type: "double precision", nullable: false),
                    IsolationWallThickness = table.Column<double>(type: "double precision", nullable: false),
                    OuterSheathWallThickness = table.Column<double>(type: "double precision", nullable: false),
                    AverageExternalDiameter = table.Column<double>(type: "double precision", nullable: false),
                    Resistance = table.Column<double>(type: "double precision", nullable: false),
                    ApproximateWeight = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    DetailImage = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AboutUsItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SubDescription = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutUsItems_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTranslations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactInformationTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactInformationId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInformationTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInformationTranslations_ContactInformations_ContactI~",
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

            migrationBuilder.CreateTable(
                name: "HistoryItemTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    HistoryItemId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "HomePageTextTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: true),
                    TextId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePageTextTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomePageTextTranslations_HomePageTexts_TextId",
                        column: x => x.TextId,
                        principalTable: "HomePageTexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomePageTextTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NavbarItemsTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NavbarItemId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavbarItemsTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NavbarItemsTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NavbarItemsTranslations_NavbarItems_NavbarItemId",
                        column: x => x.NavbarItemId,
                        principalTable: "NavbarItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PdsDocumentsTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    PdsId = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "CertificateStandarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CertificateId = table.Column<int>(type: "integer", nullable: false),
                    StandartId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateStandarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateStandarts_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateStandarts_Standarts_StandartId",
                        column: x => x.StandartId,
                        principalTable: "Standarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCertificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    CertificateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCertificates_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCertificates_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductStandarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    StandartId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStandarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStandarts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStandarts_Standarts_StandartId",
                        column: x => x.StandartId,
                        principalTable: "Standarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductStructures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    StructureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStructures_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStructures_Structures_StructureId",
                        column: x => x.StructureId,
                        principalTable: "Structures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsageLocations = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTranslations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsItems_LanguageId",
                table: "AboutUsItems",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslations_CategoryId",
                table: "CategoryTranslations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslations_LanguageId",
                table: "CategoryTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateStandarts_CertificateId",
                table: "CertificateStandarts",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateStandarts_StandartId",
                table: "CertificateStandarts",
                column: "StandartId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformationTranslations_ContactInformationId",
                table: "ContactInformationTranslations",
                column: "ContactInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInformationTranslations_LanguageId",
                table: "ContactInformationTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryItemTranslations_HistoryItemId",
                table: "HistoryItemTranslations",
                column: "HistoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryItemTranslations_LanguageId",
                table: "HistoryItemTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_HomePageTextTranslations_LanguageId",
                table: "HomePageTextTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_HomePageTextTranslations_TextId",
                table: "HomePageTextTranslations",
                column: "TextId");

            migrationBuilder.CreateIndex(
                name: "IX_NavbarItems_ParentId",
                table: "NavbarItems",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_NavbarItemsTranslations_LanguageId",
                table: "NavbarItemsTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_NavbarItemsTranslations_NavbarItemId",
                table: "NavbarItemsTranslations",
                column: "NavbarItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PdsDocumentsTranslations_LanguageId",
                table: "PdsDocumentsTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PdsDocumentsTranslations_PdsId",
                table: "PdsDocumentsTranslations",
                column: "PdsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCertificates_CertificateId",
                table: "ProductCertificates",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCertificates_ProductId",
                table: "ProductCertificates",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStandarts_ProductId",
                table: "ProductStandarts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStandarts_StandartId",
                table: "ProductStandarts",
                column: "StandartId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStructures_ProductId",
                table: "ProductStructures",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStructures_StructureId",
                table: "ProductStructures",
                column: "StructureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslations_LanguageId",
                table: "ProductTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTranslations_ProductId",
                table: "ProductTranslations",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUsItems");

            migrationBuilder.DropTable(
                name: "CategoryTranslations");

            migrationBuilder.DropTable(
                name: "CertificateStandarts");

            migrationBuilder.DropTable(
                name: "CertificateTypes");

            migrationBuilder.DropTable(
                name: "ContactInformationTranslations");

            migrationBuilder.DropTable(
                name: "HistoryItemTranslations");

            migrationBuilder.DropTable(
                name: "HomePageTextTranslations");

            migrationBuilder.DropTable(
                name: "NavbarItemsTranslations");

            migrationBuilder.DropTable(
                name: "PdsDocumentsTranslations");

            migrationBuilder.DropTable(
                name: "ProductCertificates");

            migrationBuilder.DropTable(
                name: "ProductStandarts");

            migrationBuilder.DropTable(
                name: "ProductStructures");

            migrationBuilder.DropTable(
                name: "ProductTranslations");

            migrationBuilder.DropTable(
                name: "StructureTranslations");

            migrationBuilder.DropTable(
                name: "SwiperItems");

            migrationBuilder.DropTable(
                name: "TechnicalFeatures");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ContactInformations");

            migrationBuilder.DropTable(
                name: "HistoryItems");

            migrationBuilder.DropTable(
                name: "HomePageTexts");

            migrationBuilder.DropTable(
                name: "NavbarItems");

            migrationBuilder.DropTable(
                name: "PdsDocuments");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "Standarts");

            migrationBuilder.DropTable(
                name: "Structures");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
