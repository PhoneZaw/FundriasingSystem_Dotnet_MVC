using Microsoft.EntityFrameworkCore.Migrations;

namespace FundriasingSystem.Migrations
{
    public partial class addIconToPaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethodIconUrl",
                table: "PaymentMethods",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethodIconUrl",
                table: "PaymentMethods");
        }
    }
}
