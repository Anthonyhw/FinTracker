using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class ArrumandoVoucherCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Vouchers",
                type: "VARCHAR(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Vouchers",
                type: "VARCHAR",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(80)",
                oldMaxLength: 80);
        }
    }
}
