using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalStore.Migrations
{
    public partial class namehaventtobeunique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_FirstName_MidName_LastName",
                table: "Customers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Customers_FirstName_MidName_LastName",
                table: "Customers",
                columns: new[] { "FirstName", "MidName", "LastName" },
                unique: true);
        }
    }
}
