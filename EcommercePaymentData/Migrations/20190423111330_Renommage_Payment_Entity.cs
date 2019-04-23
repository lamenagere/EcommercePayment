using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePaymentData.Migrations
{
    public partial class Renommage_Payment_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "Payments",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payments",
                newName: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "guid",
                table: "Payments",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Payments",
                newName: "Id");
        }
    }
}
