using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoADatos.Migrations
{
    /// <inheritdoc />
    public partial class NovenaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interfaz_Clases_ClaseId",
                table: "Interfaz");

            migrationBuilder.DropForeignKey(
                name: "FK_Metodos_Interfaz_InterfazId",
                table: "Metodos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interfaz",
                table: "Interfaz");

            migrationBuilder.RenameTable(
                name: "Interfaz",
                newName: "Interfazes");

            migrationBuilder.RenameIndex(
                name: "IX_Interfaz_ClaseId",
                table: "Interfazes",
                newName: "IX_Interfazes_ClaseId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TipoReferenciaId",
                table: "VariableLocales",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NamespaceId",
                table: "Clases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NamespaceId",
                table: "Interfazes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interfazes",
                table: "Interfazes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Namespaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PadreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Namespaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Namespaces_Namespaces_PadreId",
                        column: x => x.PadreId,
                        principalTable: "Namespaces",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Clases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000111"),
                column: "NamespaceId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Clases_NamespaceId",
                table: "Clases",
                column: "NamespaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Interfazes_NamespaceId",
                table: "Interfazes",
                column: "NamespaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Namespaces_PadreId",
                table: "Namespaces",
                column: "PadreId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "Namespaces");

            migrationBuilder.DropIndex(
                name: "IX_Clases_NamespaceId",
                table: "Clases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interfazes",
                table: "Interfazes");

            migrationBuilder.DropIndex(
                name: "IX_Interfazes_NamespaceId",
                table: "Interfazes");

            migrationBuilder.DropColumn(
                name: "NamespaceId",
                table: "Clases");

            migrationBuilder.DropColumn(
                name: "NamespaceId",
                table: "Interfazes");

            migrationBuilder.RenameTable(
                name: "Interfazes",
                newName: "Interfaz");

            migrationBuilder.RenameIndex(
                name: "IX_Interfazes_ClaseId",
                table: "Interfaz",
                newName: "IX_Interfaz_ClaseId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TipoReferenciaId",
                table: "VariableLocales",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interfaz",
                table: "Interfaz",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interfaz_Clases_ClaseId",
                table: "Interfaz",
                column: "ClaseId",
                principalTable: "Clases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Metodos_Interfaz_InterfazId",
                table: "Metodos",
                column: "InterfazId",
                principalTable: "Interfaz",
                principalColumn: "Id");
        }
    }
}
