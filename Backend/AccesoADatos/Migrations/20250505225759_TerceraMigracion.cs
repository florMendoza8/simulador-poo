using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AccesoADatos.Migrations
{
    /// <inheritdoc />
    public partial class TerceraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClaseBaseId",
                table: "Clases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "TipoDatos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "bool" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "byte" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "sbyte" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "char" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "decimal" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "double" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "float" },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "int" },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "uint" },
                    { new Guid("00000000-0000-0000-0000-000000000010"), "nint" },
                    { new Guid("00000000-0000-0000-0000-000000000011"), "nuint" },
                    { new Guid("00000000-0000-0000-0000-000000000012"), "long" },
                    { new Guid("00000000-0000-0000-0000-000000000013"), "ulong" },
                    { new Guid("00000000-0000-0000-0000-000000000014"), "short" },
                    { new Guid("00000000-0000-0000-0000-000000000015"), "ushort" },
                    { new Guid("00000000-0000-0000-0000-000000000016"), "object" },
                    { new Guid("00000000-0000-0000-0000-000000000017"), "string" },
                    { new Guid("00000000-0000-0000-0000-000000000018"), "delegate" },
                    { new Guid("00000000-0000-0000-0000-000000000019"), "dynamic" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clases_ClaseBaseId",
                table: "Clases",
                column: "ClaseBaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Clases_ClaseBaseId",
                table: "Clases",
                column: "ClaseBaseId",
                principalTable: "Clases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Clases_ClaseBaseId",
                table: "Clases");

            migrationBuilder.DropIndex(
                name: "IX_Clases_ClaseBaseId",
                table: "Clases");

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.DropColumn(
                name: "ClaseBaseId",
                table: "Clases");
        }
    }
}
