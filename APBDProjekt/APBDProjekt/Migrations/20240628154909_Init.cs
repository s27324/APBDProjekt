using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APBDProjekt.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Category_pk", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    KRS = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Company_pk", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Offer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Timeslot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Discount_pk", x => x.DiscountId);
                });

            migrationBuilder.CreateTable(
                name: "PrivateClient",
                columns: table => new
                {
                    PrivateClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PESEL = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PrivateClient_pk", x => x.PrivateClientId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AppUser_pk", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareSystem",
                columns: table => new
                {
                    SoftwareSystemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SoftwareSystem_pk", x => x.SoftwareSystemId);
                });

            migrationBuilder.CreateTable(
                name: "Version",
                columns: table => new
                {
                    VersionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VersionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Version_pk", x => x.VersionId);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PrivateClientId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Client_pk", x => x.ClientId);
                    table.ForeignKey(
                        name: "Client_Company",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Client_PrivateClient",
                        column: x => x.PrivateClientId,
                        principalTable: "PrivateClient",
                        principalColumn: "PrivateClientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RefreshTokenExp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pk", x => x.UserId);
                    table.ForeignKey(
                        name: "AppUser_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareSystem_Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SoftwareSystemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareSystem_Category", x => new { x.CategoryId, x.SoftwareSystemId });
                    table.ForeignKey(
                        name: "FK_SoftwareSystem_Category_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoftwareSystem_Category_SoftwareSystem_SoftwareSystemId",
                        column: x => x.SoftwareSystemId,
                        principalTable: "SoftwareSystem",
                        principalColumn: "SoftwareSystemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareSystem_Discount",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    SoftwareSystemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareSystem_Discount", x => new { x.DiscountId, x.SoftwareSystemId });
                    table.ForeignKey(
                        name: "FK_SoftwareSystem_Discount_Discount_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discount",
                        principalColumn: "DiscountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoftwareSystem_Discount_SoftwareSystem_SoftwareSystemId",
                        column: x => x.SoftwareSystemId,
                        principalTable: "SoftwareSystem",
                        principalColumn: "SoftwareSystemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareSystem_Version",
                columns: table => new
                {
                    SoftwareSystemVersionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VersionId = table.Column<int>(type: "int", nullable: false),
                    SoftwareSystemId = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SoftwareSystemVersion_pk", x => x.SoftwareSystemVersionId);
                    table.ForeignKey(
                        name: "SoftwareSystemVersion_SoftwareSystem",
                        column: x => x.SoftwareSystemId,
                        principalTable: "SoftwareSystem",
                        principalColumn: "SoftwareSystemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "SoftwareSystemVersion_Version",
                        column: x => x.VersionId,
                        principalTable: "Version",
                        principalColumn: "VersionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentCharge = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    MaxCharge = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSigned = table.Column<bool>(type: "bit", nullable: false),
                    YearsOfSupport = table.Column<int>(type: "int", nullable: false),
                    SoftwareSystemId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Contract_pk", x => x.ContractId);
                    table.ForeignKey(
                        name: "Contract_Client",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Contract_SoftwareSystem",
                        column: x => x.SoftwareSystemId,
                        principalTable: "SoftwareSystem",
                        principalColumn: "SoftwareSystemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Finance" },
                    { 2, "Education" },
                    { 3, "Utility" },
                    { 4, "Calculation" },
                    { 5, "Translation" },
                    { 6, "Image manipulation" }
                });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "CompanyId", "KRS", "Name" },
                values: new object[,]
                {
                    { 1, "449188792", "Cashzo" },
                    { 2, "109790978", "Mathico" },
                    { 3, "66882822454381", "Lingify" },
                    { 4, "668021149", "Fotofuse" }
                });

            migrationBuilder.InsertData(
                table: "Discount",
                columns: new[] { "DiscountId", "Name", "Offer", "Timeslot", "Value" },
                values: new object[,]
                {
                    { 1, "The beginning of the school year Discount", "Discount on education systems", "01-08 30-09", 10m },
                    { 2, "New Year Discount", "Discount on all systems", "01-01 16-01", 5.5m },
                    { 3, "Finance February Discount", "Discount on finance systems", "01-02 25-02", 7.5m },
                    { 4, "Utility Holiday Discount", "Discount on utility systems", "01-07 01-09", 12.5m },
                    { 5, "Calculation Systems Discount", "Discount on calculation systems", "13-01 14-03", 8.5m },
                    { 6, "Learning and finance Discount", "Discount on finance and education systems", "20-02 06-08", 6.5m }
                });

            migrationBuilder.InsertData(
                table: "PrivateClient",
                columns: new[] { "PrivateClientId", "FirstName", "LastName", "PESEL" },
                values: new object[,]
                {
                    { 1, "John", "Doe", "89110281190" },
                    { 2, "Marlena", "Tomczyk", "43071918403" },
                    { 3, "Sonia", "Duda", "76102295729" },
                    { 4, "Tadeusz", "Baran", "33110448277" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "Name" },
                values: new object[,]
                {
                    { 1, "Employee" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "SoftwareSystem",
                columns: new[] { "SoftwareSystemId", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "System to help companies with all financial issues, such as revenue, and to teach inexperienced employees.", "FinanceExpert", 54000m },
                    { 2, "System to help students understand difficult mathematical topics.", "MathsHelper", 17500m },
                    { 3, "System to help count the paychecks of employees from different departments based on hours worked and other factors.", "WageManager", 35000m },
                    { 4, "System that helps calculate very large numbers and complex data.", "Calculator++", 15000m },
                    { 5, "System to improve the tasks of the translator. In addition, equipped with a powerful database of words, synonyms, etc.", "TranslatorMax", 44000m },
                    { 6, "System to help you transform and enhance your photos with intuitive, professional editing tools.", "PixelPerfect", 65000m }
                });

            migrationBuilder.InsertData(
                table: "Version",
                columns: new[] { "VersionId", "Name", "VersionDate" },
                values: new object[,]
                {
                    { 1, "1.1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "1.0", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "0.9", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "0.8", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "0.7", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "0.6", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "ClientId", "Address", "CompanyId", "Email", "IsDeleted", "PhoneNumber", "PrivateClientId" },
                values: new object[,]
                {
                    { 1, "ul. Chmiel 47c - Zgierz, LU / 65-239", null, "doe@gmail.com", false, "+48 543772365", 1 },
                    { 2, "ul. Cybulski 81 - Ścinawa, PK / 74-648", null, "marltom@gmail.com", false, "+1 9792862590", 2 },
                    { 3, "ul. Śliwa 4/1 - Wieruszów, LU / 99-878", null, "soniaduda@gmail.com", false, "+48 852951976", 3 },
                    { 4, "al. Kostrzewa 270 - Lipiany, LB / 71-013", null, "tadbaran@gmail.com", false, "+48 904095402", 4 },
                    { 5, "pl. Szczepański 32c - Jeziorany, KP / 52-673", 1, "dir@cashzo.com", false, "+48 802107161", null },
                    { 6, "pl. Turek 05c - Polanów, ZP / 07-186", 2, "info@mathico.com", false, "+48 209770172", null },
                    { 7, "42 Ockham Road - East Witton, DL8 8TU", 3, "sales@lingify.uk", false, "+44 7994365813", null },
                    { 8, "pl. Molenda 42c - Gdynia, LD / 86-752", 4, "fotofuse@gmail.com", true, "+48 517455585", null }
                });

            migrationBuilder.InsertData(
                table: "SoftwareSystem_Category",
                columns: new[] { "CategoryId", "SoftwareSystemId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 4 },
                    { 3, 3 },
                    { 4, 1 },
                    { 4, 2 },
                    { 4, 4 },
                    { 5, 5 },
                    { 6, 6 }
                });

            migrationBuilder.InsertData(
                table: "SoftwareSystem_Discount",
                columns: new[] { "DiscountId", "SoftwareSystemId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 4 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 2, 6 },
                    { 3, 1 },
                    { 3, 3 },
                    { 4, 3 },
                    { 5, 1 },
                    { 5, 2 },
                    { 5, 4 },
                    { 6, 1 },
                    { 6, 2 },
                    { 6, 4 }
                });

            migrationBuilder.InsertData(
                table: "SoftwareSystem_Version",
                columns: new[] { "SoftwareSystemVersionId", "ReleaseDate", "SoftwareSystemId", "VersionId" },
                values: new object[,]
                {
                    { 1, new DateTime(2019, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4 },
                    { 2, new DateTime(2020, 7, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3 },
                    { 3, new DateTime(2023, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2 },
                    { 4, new DateTime(2024, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 4 },
                    { 5, new DateTime(2020, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3 },
                    { 6, new DateTime(2024, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2 },
                    { 8, new DateTime(2017, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 4 },
                    { 9, new DateTime(2022, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2 },
                    { 10, new DateTime(2015, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 6 },
                    { 11, new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2 },
                    { 12, new DateTime(2022, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 3 },
                    { 13, new DateTime(2024, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2 },
                    { 14, new DateTime(2008, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 5 },
                    { 15, new DateTime(2013, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 4 },
                    { 16, new DateTime(2019, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 3 },
                    { 17, new DateTime(2023, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 2 }
                });

            migrationBuilder.InsertData(
                table: "Contract",
                columns: new[] { "ContractId", "ClientId", "CurrentCharge", "EndDate", "IsSigned", "MaxCharge", "SoftwareSystemId", "StartDate", "YearsOfSupport" },
                values: new object[,]
                {
                    { 1, 1, 57000m, new DateTime(2022, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 57000m, 1, new DateTime(2022, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 2, 2, 0m, new DateTime(2021, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 18500m, 2, new DateTime(2021, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, 2, 18500m, new DateTime(2021, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 18500m, 2, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 4, 5, 4000m, new DateTime(2024, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 37000m, 3, new DateTime(2024, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 5, 7, 0m, new DateTime(2024, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 68000m, 6, new DateTime(2024, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_RoleId",
                table: "AppUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_CompanyId",
                table: "Client",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_PrivateClientId",
                table: "Client",
                column: "PrivateClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ClientId",
                table: "Contract",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_SoftwareSystemId",
                table: "Contract",
                column: "SoftwareSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareSystem_Category_SoftwareSystemId",
                table: "SoftwareSystem_Category",
                column: "SoftwareSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareSystem_Discount_SoftwareSystemId",
                table: "SoftwareSystem_Discount",
                column: "SoftwareSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareSystem_Version_SoftwareSystemId",
                table: "SoftwareSystem_Version",
                column: "SoftwareSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareSystem_Version_VersionId",
                table: "SoftwareSystem_Version",
                column: "VersionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "SoftwareSystem_Category");

            migrationBuilder.DropTable(
                name: "SoftwareSystem_Discount");

            migrationBuilder.DropTable(
                name: "SoftwareSystem_Version");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "SoftwareSystem");

            migrationBuilder.DropTable(
                name: "Version");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "PrivateClient");
        }
    }
}
