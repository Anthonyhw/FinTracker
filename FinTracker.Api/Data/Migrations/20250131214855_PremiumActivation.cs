using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class PremiumActivation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "ProductDuration",
                table: "Products",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PremiumExpirationDate",
                table: "IdentityUser",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductDuration",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PremiumExpirationDate",
                table: "IdentityUser");
        }
    }
}
