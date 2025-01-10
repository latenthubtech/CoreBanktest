using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoreBankerApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    IndustryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.IndustryId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerIndustries",
                columns: table => new
                {
                    CustomersCustomerId = table.Column<int>(type: "int", nullable: false),
                    IndustriesIndustryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerIndustries", x => new { x.CustomersCustomerId, x.IndustriesIndustryId });
                    table.ForeignKey(
                        name: "FK_CustomerIndustries_Customers_CustomersCustomerId",
                        column: x => x.CustomersCustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerIndustries_Industries_IndustriesIndustryId",
                        column: x => x.IndustriesIndustryId,
                        principalTable: "Industries",
                        principalColumn: "IndustryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndustryTypes",
                columns: table => new
                {
                    IndustryTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndustryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryTypes", x => x.IndustryTypeId);
                    table.ForeignKey(
                        name: "FK_IndustryTypes_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "IndustryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "AccountNumber" },
                values: new object[,]
                {
                    { 1, "1234567890" },
                    { 2, "2345678901" },
                    { 3, "3456789012" }
                });

            migrationBuilder.InsertData(
                table: "Industries",
                columns: new[] { "IndustryId", "Name" },
                values: new object[,]
                {
                    { 1, "Manufacturing" },
                    { 2, "Education" },
                    { 3, "Telecom" }
                });

            migrationBuilder.InsertData(
                table: "CustomerIndustries",
                columns: new[] { "CustomersCustomerId", "IndustriesIndustryId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "IndustryTypes",
                columns: new[] { "IndustryTypeId", "IndustryId", "Label", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Invoice Number", "Invoice Number" },
                    { 2, 1, "Quantity", "Quantity" },
                    { 3, 1, "Delivery Address", "Delivery Address" },
                    { 4, 2, "Invoice Number", "Invoice Number" },
                    { 5, 2, "Level", "Level" },
                    { 6, 2, "Course", "Course" },
                    { 7, 3, "GSM Number", "GSM Number" },
                    { 8, 3, "Network", "Level" },
                    { 9, 3, "Residential Address", "Course" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIndustries_IndustriesIndustryId",
                table: "CustomerIndustries",
                column: "IndustriesIndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustryTypes_IndustryId",
                table: "IndustryTypes",
                column: "IndustryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerIndustries");

            migrationBuilder.DropTable(
                name: "IndustryTypes");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Industries");
        }
    }
}
