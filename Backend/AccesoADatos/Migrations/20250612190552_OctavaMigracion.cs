using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoADatos.Migrations
{
    /// <inheritdoc />
    public partial class OctavaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atributos_TipoDatos_TipoDatoId",
                table: "Atributos");

            migrationBuilder.DropForeignKey(
                name: "FK_VariableLocales_TipoDatos_TipoDatoId",
                table: "VariableLocales");

            migrationBuilder.RenameColumn(
                name: "TipoDatoId",
                table: "VariableLocales",
                newName: "TipoReferenciaId");

            migrationBuilder.RenameIndex(
                name: "IX_VariableLocales_TipoDatoId",
                table: "VariableLocales",
                newName: "IX_VariableLocales_TipoReferenciaId");

            migrationBuilder.RenameColumn(
                name: "TipoDatoId",
                table: "Atributos",
                newName: "TipoReferenciaId");

            migrationBuilder.RenameIndex(
                name: "IX_Atributos_TipoDatoId",
                table: "Atributos",
                newName: "IX_Atributos_TipoReferenciaId");

            migrationBuilder.AddColumn<Guid>(
                name: "TipoInstanciaId",
                table: "VariableLocales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TipoPadreId",
                table: "TipoDatos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsEstatico",
                table: "Metodos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EsOverride",
                table: "Metodos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EsVirtual",
                table: "Metodos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "InterfazId",
                table: "Metodos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsEstatico",
                table: "Atributos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TipoInstanciaId",
                table: "Atributos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interfaz",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interfaz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interfaz_Clases_ClaseId",
                        column: x => x.ClaseId,
                        principalTable: "Clases",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ApiKeys",
                column: "Id",
                values: new object[]
                {
                    new Guid("00112233-4455-6677-8899-aabbccddeeff"),
                    new Guid("102af4be-30ea-46ee-ed49-e5d9345e3368"),
                    new Guid("12345678-90ab-cdef-1234-567890abcdef"),
                    new Guid("1a2b3c4d-5e6f-7081-92a3-b4c5d6e7f8e9"),
                    new Guid("9c0ff0b1-4abd-45c6-8a4a-831748fb7a20"),
                    new Guid("a1b2c3d4-e5f6-7890-abcd-1234567890ab"),
                    new Guid("aabbccdd-1122-3344-5566-77889900aabb"),
                    new Guid("ab12cd34-ef56-7890-1234-56789abcdeff"),
                    new Guid("c0defa55-1234-4fed-9cba-112233445566"),
                    new Guid("f3a8c091-04a9-4d88-9a1c-a0b1c2d3e4f5")
                });

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000014"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000015"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000016"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000017"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000018"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TipoDatos",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000019"),
                column: "TipoPadreId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_VariableLocales_TipoInstanciaId",
                table: "VariableLocales",
                column: "TipoInstanciaId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoDatos_TipoPadreId",
                table: "TipoDatos",
                column: "TipoPadreId");

            migrationBuilder.CreateIndex(
                name: "IX_Metodos_InterfazId",
                table: "Metodos",
                column: "InterfazId");

            migrationBuilder.CreateIndex(
                name: "IX_Atributos_TipoInstanciaId",
                table: "Atributos",
                column: "TipoInstanciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Interfaz_ClaseId",
                table: "Interfaz",
                column: "ClaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Atributos_TipoDatos_TipoInstanciaId",
                table: "Atributos",
                column: "TipoInstanciaId",
                principalTable: "TipoDatos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Atributos_TipoDatos_TipoReferenciaId",
                table: "Atributos",
                column: "TipoReferenciaId",
                principalTable: "TipoDatos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Metodos_Interfaz_InterfazId",
                table: "Metodos",
                column: "InterfazId",
                principalTable: "Interfaz",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TipoDatos_TipoDatos_TipoPadreId",
                table: "TipoDatos",
                column: "TipoPadreId",
                principalTable: "TipoDatos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariableLocales_TipoDatos_TipoInstanciaId",
                table: "VariableLocales",
                column: "TipoInstanciaId",
                principalTable: "TipoDatos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariableLocales_TipoDatos_TipoReferenciaId",
                table: "VariableLocales",
                column: "TipoReferenciaId",
                principalTable: "TipoDatos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atributos_TipoDatos_TipoInstanciaId",
                table: "Atributos");

            migrationBuilder.DropForeignKey(
                name: "FK_Atributos_TipoDatos_TipoReferenciaId",
                table: "Atributos");

            migrationBuilder.DropForeignKey(
                name: "FK_Metodos_Interfaz_InterfazId",
                table: "Metodos");

            migrationBuilder.DropForeignKey(
                name: "FK_TipoDatos_TipoDatos_TipoPadreId",
                table: "TipoDatos");

            migrationBuilder.DropForeignKey(
                name: "FK_VariableLocales_TipoDatos_TipoInstanciaId",
                table: "VariableLocales");

            migrationBuilder.DropForeignKey(
                name: "FK_VariableLocales_TipoDatos_TipoReferenciaId",
                table: "VariableLocales");

            migrationBuilder.DropTable(
                name: "ApiKeys");

            migrationBuilder.DropTable(
                name: "Interfaz");

            migrationBuilder.DropIndex(
                name: "IX_VariableLocales_TipoInstanciaId",
                table: "VariableLocales");

            migrationBuilder.DropIndex(
                name: "IX_TipoDatos_TipoPadreId",
                table: "TipoDatos");

            migrationBuilder.DropIndex(
                name: "IX_Metodos_InterfazId",
                table: "Metodos");

            migrationBuilder.DropIndex(
                name: "IX_Atributos_TipoInstanciaId",
                table: "Atributos");

            migrationBuilder.DropColumn(
                name: "TipoInstanciaId",
                table: "VariableLocales");

            migrationBuilder.DropColumn(
                name: "TipoPadreId",
                table: "TipoDatos");

            migrationBuilder.DropColumn(
                name: "EsEstatico",
                table: "Metodos");

            migrationBuilder.DropColumn(
                name: "EsOverride",
                table: "Metodos");

            migrationBuilder.DropColumn(
                name: "EsVirtual",
                table: "Metodos");

            migrationBuilder.DropColumn(
                name: "InterfazId",
                table: "Metodos");

            migrationBuilder.DropColumn(
                name: "EsEstatico",
                table: "Atributos");

            migrationBuilder.DropColumn(
                name: "TipoInstanciaId",
                table: "Atributos");

            migrationBuilder.RenameColumn(
                name: "TipoReferenciaId",
                table: "VariableLocales",
                newName: "TipoDatoId");

            migrationBuilder.RenameIndex(
                name: "IX_VariableLocales_TipoReferenciaId",
                table: "VariableLocales",
                newName: "IX_VariableLocales_TipoDatoId");

            migrationBuilder.RenameColumn(
                name: "TipoReferenciaId",
                table: "Atributos",
                newName: "TipoDatoId");

            migrationBuilder.RenameIndex(
                name: "IX_Atributos_TipoReferenciaId",
                table: "Atributos",
                newName: "IX_Atributos_TipoDatoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Atributos_TipoDatos_TipoDatoId",
                table: "Atributos",
                column: "TipoDatoId",
                principalTable: "TipoDatos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VariableLocales_TipoDatos_TipoDatoId",
                table: "VariableLocales",
                column: "TipoDatoId",
                principalTable: "TipoDatos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
