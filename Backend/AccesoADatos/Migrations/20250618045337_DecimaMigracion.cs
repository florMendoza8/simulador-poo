using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoADatos.Migrations
{
    /// <inheritdoc />
    public partial class DecimaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Namespaces_NamespaceId",
                table: "Clases");

            migrationBuilder.DropForeignKey(
                name: "FK_Interfazes_Clases_ClaseId",
                table: "Interfazes");

            migrationBuilder.DropForeignKey(
                name: "FK_Interfazes_Namespaces_NamespaceId",
                table: "Interfazes");

            migrationBuilder.DropForeignKey(
                name: "FK_Metodos_Interfazes_InterfazId",
                table: "Metodos");

            migrationBuilder.DropForeignKey(
                name: "FK_Namespaces_Namespaces_PadreId",
                table: "Namespaces");

            migrationBuilder.DropIndex(
                name: "IX_Metodos_InterfazId",
                table: "Metodos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Namespaces",
                table: "Namespaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interfazes",
                table: "Interfazes");

            migrationBuilder.DropColumn(
                name: "InterfazId",
                table: "Metodos");

            migrationBuilder.RenameTable(
                name: "Namespaces",
                newName: "Namespace");

            migrationBuilder.RenameTable(
                name: "Interfazes",
                newName: "Interfaz");

            migrationBuilder.RenameIndex(
                name: "IX_Namespaces_PadreId",
                table: "Namespace",
                newName: "IX_Namespace_PadreId");

            migrationBuilder.RenameIndex(
                name: "IX_Interfazes_NamespaceId",
                table: "Interfaz",
                newName: "IX_Interfaz_NamespaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Interfazes_ClaseId",
                table: "Interfaz",
                newName: "IX_Interfaz_ClaseId");

            migrationBuilder.AddColumn<Guid>(
                name: "NamespaceId1",
                table: "Clases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Namespace",
                table: "Namespace",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interfaz",
                table: "Interfaz",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InterfazMetodo",
                columns: table => new
                {
                    FirmaMetodosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterfazId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterfazMetodo", x => new { x.FirmaMetodosId, x.InterfazId });
                    table.ForeignKey(
                        name: "FK_InterfazMetodo_Interfaz_InterfazId",
                        column: x => x.InterfazId,
                        principalTable: "Interfaz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterfazMetodo_Metodos_FirmaMetodosId",
                        column: x => x.FirmaMetodosId,
                        principalTable: "Metodos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Clases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000111"),
                column: "NamespaceId1",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Clases_NamespaceId1",
                table: "Clases",
                column: "NamespaceId1");

            migrationBuilder.CreateIndex(
                name: "IX_InterfazMetodo_InterfazId",
                table: "InterfazMetodo",
                column: "InterfazId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Namespace_NamespaceId",
                table: "Clases",
                column: "NamespaceId",
                principalTable: "Namespace",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Namespace_NamespaceId1",
                table: "Clases",
                column: "NamespaceId1",
                principalTable: "Namespace",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interfaz_Clases_ClaseId",
                table: "Interfaz",
                column: "ClaseId",
                principalTable: "Clases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interfaz_Namespace_NamespaceId",
                table: "Interfaz",
                column: "NamespaceId",
                principalTable: "Namespace",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Namespace_Namespace_PadreId",
                table: "Namespace",
                column: "PadreId",
                principalTable: "Namespace",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Namespace_NamespaceId",
                table: "Clases");

            migrationBuilder.DropForeignKey(
                name: "FK_Clases_Namespace_NamespaceId1",
                table: "Clases");

            migrationBuilder.DropForeignKey(
                name: "FK_Interfaz_Clases_ClaseId",
                table: "Interfaz");

            migrationBuilder.DropForeignKey(
                name: "FK_Interfaz_Namespace_NamespaceId",
                table: "Interfaz");

            migrationBuilder.DropForeignKey(
                name: "FK_Namespace_Namespace_PadreId",
                table: "Namespace");

            migrationBuilder.DropTable(
                name: "InterfazMetodo");

            migrationBuilder.DropIndex(
                name: "IX_Clases_NamespaceId1",
                table: "Clases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Namespace",
                table: "Namespace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interfaz",
                table: "Interfaz");

            migrationBuilder.DropColumn(
                name: "NamespaceId1",
                table: "Clases");

            migrationBuilder.RenameTable(
                name: "Namespace",
                newName: "Namespaces");

            migrationBuilder.RenameTable(
                name: "Interfaz",
                newName: "Interfazes");

            migrationBuilder.RenameIndex(
                name: "IX_Namespace_PadreId",
                table: "Namespaces",
                newName: "IX_Namespaces_PadreId");

            migrationBuilder.RenameIndex(
                name: "IX_Interfaz_NamespaceId",
                table: "Interfazes",
                newName: "IX_Interfazes_NamespaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Interfaz_ClaseId",
                table: "Interfazes",
                newName: "IX_Interfazes_ClaseId");

            migrationBuilder.AddColumn<Guid>(
                name: "InterfazId",
                table: "Metodos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Namespaces",
                table: "Namespaces",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interfazes",
                table: "Interfazes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Metodos_InterfazId",
                table: "Metodos",
                column: "InterfazId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clases_Namespaces_NamespaceId",
                table: "Clases",
                column: "NamespaceId",
                principalTable: "Namespaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Interfazes_Clases_ClaseId",
                table: "Interfazes",
                column: "ClaseId",
                principalTable: "Clases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interfazes_Namespaces_NamespaceId",
                table: "Interfazes",
                column: "NamespaceId",
                principalTable: "Namespaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Metodos_Interfazes_InterfazId",
                table: "Metodos",
                column: "InterfazId",
                principalTable: "Interfazes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Namespaces_Namespaces_PadreId",
                table: "Namespaces",
                column: "PadreId",
                principalTable: "Namespaces",
                principalColumn: "Id");
        }
    }
}
