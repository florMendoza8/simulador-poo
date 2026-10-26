using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoADatos.Migrations
{
    /// <inheritdoc />
    public partial class SeptimaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ClaseBaseId",
                table: "Clases",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Clases",
                columns: new[] { "Id", "ClaseBaseId", "EsAbstract", "EsSealed", "Nombre" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000111"), null, false, false, "Object" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000111"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ClaseBaseId",
                table: "Clases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
