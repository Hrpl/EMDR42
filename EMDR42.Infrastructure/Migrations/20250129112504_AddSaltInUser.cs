using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMDR42.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSaltInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");
        }
    }
}
