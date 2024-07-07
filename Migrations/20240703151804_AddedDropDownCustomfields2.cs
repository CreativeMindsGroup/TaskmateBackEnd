using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMate.Migrations
{
    /// <inheritdoc />
    public partial class AddedDropDownCustomfields2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DropDownOptions_CustomFields_CustomFieldsId",
                table: "DropDownOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DropDownOptions_DropDowns_DropDownId",
                table: "DropDownOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DropDowns_CustomFields_CustomFieldsId",
                table: "DropDowns");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomFieldsId",
                table: "DropDowns",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DropDownId",
                table: "DropDownOptions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomFieldsId",
                table: "DropDownOptions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "DropDownOptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_DropDownOptions_CustomFields_CustomFieldsId",
                table: "DropDownOptions",
                column: "CustomFieldsId",
                principalTable: "CustomFields",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DropDownOptions_DropDowns_DropDownId",
                table: "DropDownOptions",
                column: "DropDownId",
                principalTable: "DropDowns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DropDowns_CustomFields_CustomFieldsId",
                table: "DropDowns",
                column: "CustomFieldsId",
                principalTable: "CustomFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DropDownOptions_CustomFields_CustomFieldsId",
                table: "DropDownOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DropDownOptions_DropDowns_DropDownId",
                table: "DropDownOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DropDowns_CustomFields_CustomFieldsId",
                table: "DropDowns");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "DropDownOptions");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomFieldsId",
                table: "DropDowns",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "DropDownId",
                table: "DropDownOptions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomFieldsId",
                table: "DropDownOptions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DropDownOptions_CustomFields_CustomFieldsId",
                table: "DropDownOptions",
                column: "CustomFieldsId",
                principalTable: "CustomFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DropDownOptions_DropDowns_DropDownId",
                table: "DropDownOptions",
                column: "DropDownId",
                principalTable: "DropDowns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DropDowns_CustomFields_CustomFieldsId",
                table: "DropDowns",
                column: "CustomFieldsId",
                principalTable: "CustomFields",
                principalColumn: "Id");
        }
    }
}
