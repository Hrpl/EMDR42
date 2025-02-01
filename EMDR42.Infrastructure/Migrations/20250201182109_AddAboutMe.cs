using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMDR42.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutMe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "photo",
                table: "user_profile",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "about_me",
                table: "user_profile",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "about_me",
                table: "user_profile");

            migrationBuilder.AlterColumn<string>(
                name: "photo",
                table: "user_profile",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
