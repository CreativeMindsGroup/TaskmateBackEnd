using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMate.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDropdown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DropDownOptions_CustomFields_CustomFieldsId",
                table: "DropDownOptions");

            migrationBuilder.DropIndex(
                name: "IX_DropDownOptions_CustomFieldsId",
                table: "DropDownOptions");

            migrationBuilder.DropIndex(
                name: "IX_DropDownOptions_DropDownId",
                table: "DropDownOptions");

            migrationBuilder.DropColumn(
                name: "CustomFieldsId",
                table: "DropDownOptions");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "DropDowns",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptionName",
                table: "DropDowns",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedId",
                table: "DropDowns",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DropDownOptions_DropDownId",
                table: "DropDownOptions",
                column: "DropDownId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DropDownOptions_DropDownId",
                table: "DropDownOptions");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "DropDowns");

            migrationBuilder.DropColumn(
                name: "OptionName",
                table: "DropDowns");

            migrationBuilder.DropColumn(
                name: "SelectedId",
                table: "DropDowns");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomFieldsId",
                table: "DropDownOptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DropDownOptions_CustomFieldsId",
                table: "DropDownOptions",
                column: "CustomFieldsId");

            migrationBuilder.CreateIndex(
                name: "IX_DropDownOptions_DropDownId",
                table: "DropDownOptions",
                column: "DropDownId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DropDownOptions_CustomFields_CustomFieldsId",
                table: "DropDownOptions",
                column: "CustomFieldsId",
                principalTable: "CustomFields",
                principalColumn: "Id");
        }
    }
}
