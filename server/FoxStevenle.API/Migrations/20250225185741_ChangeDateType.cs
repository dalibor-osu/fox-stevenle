using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoxStevenle.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "date",
                schema: "fox_stevenle",
                table: "daily_quizzes",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date",
                schema: "fox_stevenle",
                table: "daily_quizzes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
