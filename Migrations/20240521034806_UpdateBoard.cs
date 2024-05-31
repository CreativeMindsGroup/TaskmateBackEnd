using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMate.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "Boards",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Theme",
                table: "Boards");
        }
    }
}
