using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTuEvento_.Migrations
{
    /// <inheritdoc />
    public partial class AgregacionDeRelacionesEntreClases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_boletos_personas_PersonaId",
                table: "boletos");

            migrationBuilder.DropForeignKey(
                name: "FK_eventos_categoriaEventos_CategoriaEventoIdCategoriaEvento",
                table: "eventos");

            migrationBuilder.DropIndex(
                name: "IX_eventos_CategoriaEventoIdCategoriaEvento",
                table: "eventos");

            migrationBuilder.DropColumn(
                name: "CategoriaEvento",
                table: "eventos");

            migrationBuilder.DropColumn(
                name: "CategoriaEventoIdCategoriaEvento",
                table: "eventos");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaEventoId",
                table: "eventos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NombreComprador",
                table: "boletos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_eventos_CategoriaEventoId",
                table: "eventos",
                column: "CategoriaEventoId");

            migrationBuilder.AddForeignKey(
                name: "FK_boletos_personas_PersonaId",
                table: "boletos",
                column: "PersonaId",
                principalTable: "personas",
                principalColumn: "PersonaId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_eventos_categoriaEventos_CategoriaEventoId",
                table: "eventos",
                column: "CategoriaEventoId",
                principalTable: "categoriaEventos",
                principalColumn: "IdCategoriaEvento",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_boletos_personas_PersonaId",
                table: "boletos");

            migrationBuilder.DropForeignKey(
                name: "FK_eventos_categoriaEventos_CategoriaEventoId",
                table: "eventos");

            migrationBuilder.DropIndex(
                name: "IX_eventos_CategoriaEventoId",
                table: "eventos");

            migrationBuilder.DropColumn(
                name: "CategoriaEventoId",
                table: "eventos");

            migrationBuilder.DropColumn(
                name: "NombreComprador",
                table: "boletos");

            migrationBuilder.AddColumn<string>(
                name: "CategoriaEvento",
                table: "eventos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaEventoIdCategoriaEvento",
                table: "eventos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_eventos_CategoriaEventoIdCategoriaEvento",
                table: "eventos",
                column: "CategoriaEventoIdCategoriaEvento");

            migrationBuilder.AddForeignKey(
                name: "FK_boletos_personas_PersonaId",
                table: "boletos",
                column: "PersonaId",
                principalTable: "personas",
                principalColumn: "PersonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_eventos_categoriaEventos_CategoriaEventoIdCategoriaEvento",
                table: "eventos",
                column: "CategoriaEventoIdCategoriaEvento",
                principalTable: "categoriaEventos",
                principalColumn: "IdCategoriaEvento");
        }
    }
}
