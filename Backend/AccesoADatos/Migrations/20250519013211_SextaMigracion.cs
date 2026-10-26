using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoADatos.Migrations
{
    /// <inheritdoc />
    public partial class SextaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Clases_ClaseBaseId",
                table: "Clases");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClaseBaseId",
                table: "Clases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Clases_ClaseBaseId",
                table: "Clases",
                column: "ClaseBaseId",
                principalTable: "Clases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Clases_ClaseBaseId",
                table: "Clases");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClaseBaseId",
                table: "Clases",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Clases_ClaseBaseId",
                table: "Clases",
                column: "ClaseBaseId",
                principalTable: "Clases",
                principalColumn: "Id");
        }
    }
}
