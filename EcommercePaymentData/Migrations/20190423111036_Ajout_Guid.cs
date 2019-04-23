using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePaymentData.Migrations
{
    public partial class Ajout_Guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Payments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Payments");
        }
    }
}
