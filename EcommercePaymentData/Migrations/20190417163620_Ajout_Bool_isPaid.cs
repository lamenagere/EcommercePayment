using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePaymentData.Migrations
{
    public partial class Ajout_Bool_isPaid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPaid",
                table: "Payments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPaid",
                table: "Payments");
        }
    }
}
