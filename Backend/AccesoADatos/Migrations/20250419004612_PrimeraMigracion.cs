using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccesoADatos.Migrations
{
    /// <inheritdoc />
    public partial class PrimeraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EsAbstract = table.Column<bool>(type: "bit", nullable: true),
                    EsSealed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoDatos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDatos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Atributos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoDatoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Accesibilidad = table.Column<int>(type: "int", nullable: true),
                    ClaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atributos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atributos_Clases_ClaseId",
                        column: x => x.ClaseId,
                        principalTable: "Clases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Atributos_TipoDatos_TipoDatoId",
                        column: x => x.TipoDatoId,
                        principalTable: "TipoDatos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Metodos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoRetornoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EsAbstract = table.Column<bool>(type: "bit", nullable: true),
                    EsSealed = table.Column<bool>(type: "bit", nullable: true),
                    Accesibilidad = table.Column<int>(type: "int", nullable: true),
                    ClaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MetodoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metodos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metodos_Clases_ClaseId",
                        column: x => x.ClaseId,
                        principalTable: "Clases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Metodos_Metodos_MetodoId",
                        column: x => x.MetodoId,
                        principalTable: "Metodos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Metodos_TipoDatos_TipoRetornoId",
                        column: x => x.TipoRetornoId,
                        principalTable: "TipoDatos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Parametros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoDatoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MetodoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parametros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parametros_Metodos_MetodoId",
                        column: x => x.MetodoId,
                        principalTable: "Metodos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parametros_TipoDatos_TipoDatoId",
                        column: x => x.TipoDatoId,
                        principalTable: "TipoDatos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VariableLocales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetodoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariableLocales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariableLocales_Metodos_MetodoId",
                        column: x => x.MetodoId,
                        principalTable: "Metodos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atributos_ClaseId",
                table: "Atributos",
                column: "ClaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Atributos_TipoDatoId",
                table: "Atributos",
                column: "TipoDatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Metodos_ClaseId",
                table: "Metodos",
                column: "ClaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Metodos_MetodoId",
                table: "Metodos",
                column: "MetodoId");

            migrationBuilder.CreateIndex(
                name: "IX_Metodos_TipoRetornoId",
                table: "Metodos",
                column: "TipoRetornoId");

            migrationBuilder.CreateIndex(
                name: "IX_Parametros_MetodoId",
                table: "Parametros",
                column: "MetodoId");

            migrationBuilder.CreateIndex(
                name: "IX_Parametros_TipoDatoId",
                table: "Parametros",
                column: "TipoDatoId");

            migrationBuilder.CreateIndex(
                name: "IX_VariableLocales_MetodoId",
                table: "VariableLocales",
                column: "MetodoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atributos");

            migrationBuilder.DropTable(
                name: "Parametros");

            migrationBuilder.DropTable(
                name: "VariableLocales");

            migrationBuilder.DropTable(
                name: "Metodos");

            migrationBuilder.DropTable(
                name: "Clases");

            migrationBuilder.DropTable(
                name: "TipoDatos");
        }
    }
}
