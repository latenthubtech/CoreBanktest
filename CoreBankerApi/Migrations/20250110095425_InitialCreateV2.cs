using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoreBankerApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerIndustries");

            migrationBuilder.AddColumn<int>(
                name: "IndustryId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                column: "IndustryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "IndustryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3,
                column: "IndustryId",
                value: 3);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IndustryId",
                table: "Customers",
                column: "IndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Industries_IndustryId",
                table: "Customers",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "IndustryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Industries_IndustryId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_IndustryId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                table: "Customers");

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

            migrationBuilder.InsertData(
                table: "CustomerIndustries",
                columns: new[] { "CustomersCustomerId", "IndustriesIndustryId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIndustries_IndustriesIndustryId",
                table: "CustomerIndustries",
                column: "IndustriesIndustryId");
        }
    }
}
