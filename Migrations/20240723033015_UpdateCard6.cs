using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMate.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCard6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DropDownOptions_DropDowns_DropDownId",
                table: "DropDownOptions");

            migrationBuilder.AddForeignKey(
                name: "FK_DropDownOptions_DropDowns_DropDownId",
                table: "DropDownOptions",
                column: "DropDownId",
                principalTable: "DropDowns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DropDownOptions_DropDowns_DropDownId",
                table: "DropDownOptions");

            migrationBuilder.AddForeignKey(
                name: "FK_DropDownOptions_DropDowns_DropDownId",
                table: "DropDownOptions",
                column: "DropDownId",
                principalTable: "DropDowns",
                principalColumn: "Id");
        }
    }
}
