using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BoatSea.Migrations
{
    public partial class init7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrajanjeUDanima",
                table: "Rents");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumKrajaIznajmljivanja",
                table: "Rents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatumKrajaIznajmljivanja",
                table: "Rents");

            migrationBuilder.AddColumn<int>(
                name: "TrajanjeUDanima",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
