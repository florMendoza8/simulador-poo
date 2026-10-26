using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoADatos.Migrations
{
    /// <inheritdoc />
    public partial class SegundaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atributos_Clases_ClaseId",
                table: "Atributos");

            migrationBuilder.DropForeignKey(
                name: "FK_Atributos_TipoDatos_TipoDatoId",
                table: "Atributos");

            migrationBuilder.DropForeignKey(
                name: "FK_Metodos_Clases_ClaseId",
                table: "Metodos");

            migrationBuilder.DropForeignKey(
                name: "FK_Metodos_Metodos_MetodoId",
                table: "Metodos");

            migrationBuilder.DropForeignKey(
                name: "FK_Metodos_TipoDatos_TipoRetornoId",
                table: "Metodos");

            migrationBuilder.DropForeignKey(
                name: "FK_Parametros_Metodos_MetodoId",
                table: "Parametros");

            migrationBuilder.DropForeignKey(
                name: "FK_Parametros_TipoDatos_TipoDatoId",
                table: "Parametros");

            migrationBuilder.DropForeignKey(
                name: "FK_VariableLocales_Metodos_MetodoId",
                table: "VariableLocales");

            migrationBuilder.DropIndex(
                name: "IX_Metodos_MetodoId",
                table: "Metodos");

            migrationBuilder.DropColumn(
                name: "MetodoId",
                table: "Metodos");

            migrationBuilder.AddColumn<Guid>(
                name: "TipoDatoId",
                table: "VariableLocales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Valor",
                table: "VariableLocales",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EsAbstract",
                table: "Metodos",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EsSealed",
                table: "Clases",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EsAbstract",
                table: "Clases",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Accesibilidad",
                table: "Atributos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MetodoMetodo",
                columns: table => new
                {
                    MetodoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MetodosInvocadosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodoMetodo", x => new { x.MetodoId, x.MetodosInvocadosId });
                    table.ForeignKey(
                        name: "FK_MetodoMetodo_Metodos_MetodoId",
                        column: x => x.MetodoId,
                        principalTable: "Metodos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MetodoMetodo_Metodos_MetodosInvocadosId",
                        column: x => x.MetodosInvocadosId,
                        principalTable: "Metodos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VariableLocales_TipoDatoId",
                table: "VariableLocales",
                column: "TipoDatoId");

            migrationBuilder.CreateIndex(
                name: "IX_MetodoMetodo_MetodosInvocadosId",
                table: "MetodoMetodo",
                column: "MetodosInvocadosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Atributos_Clases_ClaseId",
                table: "Atributos",
                column: "ClaseId",
                principalTable: "Clases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Atributos_TipoDatos_TipoDatoId",
                table: "Atributos",
                column: "TipoDatoId",
                principalTable: "TipoDatos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Metodos_Clases_ClaseId",
                table: "Metodos",
                column: "ClaseId",
                principalTable: "Clases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Metodos_TipoDatos_TipoRetornoId",
                table: "Metodos",
                column: "TipoRetornoId",
                principalTable: "TipoDatos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parametros_Metodos_MetodoId",
                table: "Parametros",
                column: "MetodoId",
                principalTable: "Metodos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parametros_TipoDatos_TipoDatoId",
                table: "Parametros",
                column: "TipoDatoId",
                principalTable: "TipoDatos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VariableLocales_Metodos_MetodoId",
                table: "VariableLocales",
                column: "MetodoId",
                principalTable: "Metodos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariableLocales_TipoDatos_TipoDatoId",
                table: "VariableLocales",
                column: "TipoDatoId",
                principalTable: "TipoDatos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atributos_Clases_ClaseId",
                table: "Atributos");

            migrationBuilder.DropForeignKey(
                name: "FK_Atributos_TipoDatos_TipoDatoId",
                table: "Atributos");

            migrationBuilder.DropForeignKey(
                name: "FK_Metodos_Clases_ClaseId",
                table: "Metodos");

            migrationBuilder.DropForeignKey(
                name: "FK_Metodos_TipoDatos_TipoRetornoId",
                table: "Metodos");

            migrationBuilder.DropForeignKey(
                name: "FK_Parametros_Metodos_MetodoId",
                table: "Parametros");

            migrationBuilder.DropForeignKey(
                name: "FK_Parametros_TipoDatos_TipoDatoId",
                table: "Parametros");

            migrationBuilder.DropForeignKey(
                name: "FK_VariableLocales_Metodos_MetodoId",
                table: "VariableLocales");

            migrationBuilder.DropForeignKey(
                name: "FK_VariableLocales_TipoDatos_TipoDatoId",
                table: "VariableLocales");

            migrationBuilder.DropTable(
                name: "MetodoMetodo");

            migrationBuilder.DropIndex(
                name: "IX_VariableLocales_TipoDatoId",
                table: "VariableLocales");

            migrationBuilder.DropColumn(
                name: "TipoDatoId",
                table: "VariableLocales");

            migrationBuilder.DropColumn(
                name: "Valor",
                table: "VariableLocales");

            migrationBuilder.AlterColumn<bool>(
                name: "EsAbstract",
                table: "Metodos",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<Guid>(
                name: "MetodoId",
                table: "Metodos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EsSealed",
                table: "Clases",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "EsAbstract",
                table: "Clases",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Accesibilidad",
                table: "Atributos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Metodos_MetodoId",
                table: "Metodos",
                column: "MetodoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Atributos_Clases_ClaseId",
                table: "Atributos",
                column: "ClaseId",
                principalTable: "Clases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Atributos_TipoDatos_TipoDatoId",
                table: "Atributos",
                column: "TipoDatoId",
                principalTable: "TipoDatos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Metodos_Clases_ClaseId",
                table: "Metodos",
                column: "ClaseId",
                principalTable: "Clases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Metodos_Metodos_MetodoId",
                table: "Metodos",
                column: "MetodoId",
                principalTable: "Metodos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Metodos_TipoDatos_TipoRetornoId",
                table: "Metodos",
                column: "TipoRetornoId",
                principalTable: "TipoDatos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parametros_Metodos_MetodoId",
                table: "Parametros",
                column: "MetodoId",
                principalTable: "Metodos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parametros_TipoDatos_TipoDatoId",
                table: "Parametros",
                column: "TipoDatoId",
                principalTable: "TipoDatos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariableLocales_Metodos_MetodoId",
                table: "VariableLocales",
                column: "MetodoId",
                principalTable: "Metodos",
                principalColumn: "Id");
        }
    }
}
