using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiTenantApp.Migrations
{
    /// <inheritdoc />
    public partial class customerShowed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CustomerShowedUp",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerShowedUp",
                table: "Customers");
        }
    }
}
