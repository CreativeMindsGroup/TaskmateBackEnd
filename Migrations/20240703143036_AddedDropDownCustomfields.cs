using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMate.Migrations
{
    /// <inheritdoc />
    public partial class AddedDropDownCustomfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "CustomFieldsNumbers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DropDowns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CustomFieldsId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModiffiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropDowns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropDowns_CustomFields_CustomFieldsId",
                        column: x => x.CustomFieldsId,
                        principalTable: "CustomFields",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DropDownOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OptionName = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: true),
                    DropDownId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomFieldsId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModiffiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropDownOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropDownOptions_CustomFields_CustomFieldsId",
                        column: x => x.CustomFieldsId,
                        principalTable: "CustomFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DropDownOptions_DropDowns_DropDownId",
                        column: x => x.DropDownId,
                        principalTable: "DropDowns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DropDownOptions_CustomFieldsId",
                table: "DropDownOptions",
                column: "CustomFieldsId");

            migrationBuilder.CreateIndex(
                name: "IX_DropDownOptions_DropDownId",
                table: "DropDownOptions",
                column: "DropDownId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DropDowns_CustomFieldsId",
                table: "DropDowns",
                column: "CustomFieldsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DropDownOptions");

            migrationBuilder.DropTable(
                name: "DropDowns");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "CustomFieldsNumbers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
