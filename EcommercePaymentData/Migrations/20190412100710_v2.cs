using Microsoft.EntityFrameworkCore.Migrations;

namespace EcommercePaymentData.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "paymentAmount",
                table: "Payments",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "paymentAmount",
                table: "Payments",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
