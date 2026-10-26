using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoADatos.Migrations
{
    /// <inheritdoc />
    public partial class CuartaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClaseId1",
                table: "Metodos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metodos_ClaseId1",
                table: "Metodos",
                column: "ClaseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Metodos_Clases_ClaseId1",
                table: "Metodos",
                column: "ClaseId1",
                principalTable: "Clases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Metodos_Clases_ClaseId1",
                table: "Metodos");

            migrationBuilder.DropIndex(
                name: "IX_Metodos_ClaseId1",
                table: "Metodos");

            migrationBuilder.DropColumn(
                name: "ClaseId1",
                table: "Metodos");
        }
    }
}
