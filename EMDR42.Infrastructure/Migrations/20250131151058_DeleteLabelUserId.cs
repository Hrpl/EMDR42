using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMDR42.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteLabelUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Sessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
