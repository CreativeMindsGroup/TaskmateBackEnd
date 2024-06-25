using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMate.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomFieldsTexts_CustomFieldsId",
                table: "CustomFieldsTexts");

            migrationBuilder.DropIndex(
                name: "IX_CustomFieldsNumbers_CustomFieldsId",
                table: "CustomFieldsNumbers");

            migrationBuilder.DropIndex(
                name: "IX_CustomFieldsDates_CustomFieldsId",
                table: "CustomFieldsDates");

            migrationBuilder.DropIndex(
                name: "IX_CustomFieldsCheckboxes_CustomFieldsId",
                table: "CustomFieldsCheckboxes");

            migrationBuilder.DropIndex(
                name: "IX_CustomFields_CardId",
                table: "CustomFields");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CustomFields");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CustomFields");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "CustomFieldsNumbers",
                type: "text",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CustomFieldsNumbers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CustomFieldsCheckboxes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Checkbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    CustomFieldsId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModiffiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkbox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Checkbox_CustomFields_CustomFieldsId",
                        column: x => x.CustomFieldsId,
                        principalTable: "CustomFields",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldsTexts_CustomFieldsId",
                table: "CustomFieldsTexts",
                column: "CustomFieldsId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldsNumbers_CustomFieldsId",
                table: "CustomFieldsNumbers",
                column: "CustomFieldsId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldsDates_CustomFieldsId",
                table: "CustomFieldsDates",
                column: "CustomFieldsId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldsCheckboxes_CustomFieldsId",
                table: "CustomFieldsCheckboxes",
                column: "CustomFieldsId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFields_CardId",
                table: "CustomFields",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Checkbox_CustomFieldsId",
                table: "Checkbox",
                column: "CustomFieldsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checkbox");

            migrationBuilder.DropIndex(
                name: "IX_CustomFieldsTexts_CustomFieldsId",
                table: "CustomFieldsTexts");

            migrationBuilder.DropIndex(
                name: "IX_CustomFieldsNumbers_CustomFieldsId",
                table: "CustomFieldsNumbers");

            migrationBuilder.DropIndex(
                name: "IX_CustomFieldsDates_CustomFieldsId",
                table: "CustomFieldsDates");

            migrationBuilder.DropIndex(
                name: "IX_CustomFieldsCheckboxes_CustomFieldsId",
                table: "CustomFieldsCheckboxes");

            migrationBuilder.DropIndex(
                name: "IX_CustomFields_CardId",
                table: "CustomFields");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CustomFieldsNumbers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CustomFieldsCheckboxes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Number",
                table: "CustomFieldsNumbers",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CustomFields",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "CustomFields",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldsTexts_CustomFieldsId",
                table: "CustomFieldsTexts",
                column: "CustomFieldsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldsNumbers_CustomFieldsId",
                table: "CustomFieldsNumbers",
                column: "CustomFieldsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldsDates_CustomFieldsId",
                table: "CustomFieldsDates",
                column: "CustomFieldsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFieldsCheckboxes_CustomFieldsId",
                table: "CustomFieldsCheckboxes",
                column: "CustomFieldsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFields_CardId",
                table: "CustomFields",
                column: "CardId");
        }
    }
}
