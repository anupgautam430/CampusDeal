using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CampusDeal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class productSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PNO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    PriceTotal = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "Id", "Description", "PNO", "Price", "PriceTotal", "Seller", "Title" },
                values: new object[,]
                {
                    { 1, "the maths book", "A01", 400.0, 400.0, "john wick", "Book" },
                    { 2, "the maths notebook", "A02", 200.0, 200.0, "john snow", "notebook" },
                    { 3, "the kit for science lab", "A03", 150.0, 150.0, "john cena", " Science lab kit" },
                    { 4, "laptop in a very good condition. acer aspire 5 250GB ssd, 1 tb HDD, intel5 7gen 4 GB graphics  ", "A04", 40000.0, 39000.0, "billie", "Laptop" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
