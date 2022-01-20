﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsArchive.Infrastructure.Persistence.Migrations
{
    public partial class Migration_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_LocalizedStrings_NameId",
                table: "ProductCategories");

            migrationBuilder.AlterColumn<Guid>(
                name: "NameId",
                table: "ProductCategories",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_LocalizedStrings_NameId",
                table: "ProductCategories",
                column: "NameId",
                principalTable: "LocalizedStrings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_LocalizedStrings_NameId",
                table: "ProductCategories");

            migrationBuilder.AlterColumn<Guid>(
                name: "NameId",
                table: "ProductCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_LocalizedStrings_NameId",
                table: "ProductCategories",
                column: "NameId",
                principalTable: "LocalizedStrings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
