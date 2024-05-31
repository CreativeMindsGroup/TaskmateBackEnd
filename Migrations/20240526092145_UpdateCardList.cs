using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMate.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCardList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "CardLists",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "CardLists");
        }
    }
}
