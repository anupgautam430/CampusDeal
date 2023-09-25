using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusDeal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class postal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "postalCode",
                table: "OrderHeaders",
                newName: "PostalCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "OrderHeaders",
                newName: "postalCode");
        }
    }
}
