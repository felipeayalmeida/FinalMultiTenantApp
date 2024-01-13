using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenantApp.Migrations
{
    /// <inheritdoc />
    public partial class IncludingTenantIdOnCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Customers");
        }
    }
}
