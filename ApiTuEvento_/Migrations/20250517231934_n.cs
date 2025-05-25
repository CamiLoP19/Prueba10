using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTuEvento_.Migrations
{
    /// <inheritdoc />
    public partial class n : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_boletos_personas_UsuarioPersonaId",
                table: "boletos");

            migrationBuilder.DropIndex(
                name: "IX_boletos_UsuarioPersonaId",
                table: "boletos");

            migrationBuilder.DropColumn(
                name: "Cedula",
                table: "personas");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "personas");

            migrationBuilder.DropColumn(
                name: "UsuarioPersonaId",
                table: "boletos");

            migrationBuilder.CreateIndex(
                name: "IX_boletos_PersonaId",
                table: "boletos",
                column: "PersonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_boletos_personas_PersonaId",
                table: "boletos",
                column: "PersonaId",
                principalTable: "personas",
                principalColumn: "PersonaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_boletos_personas_PersonaId",
                table: "boletos");

            migrationBuilder.DropIndex(
                name: "IX_boletos_PersonaId",
                table: "boletos");

            migrationBuilder.AddColumn<double>(
                name: "Cedula",
                table: "personas",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "personas",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioPersonaId",
                table: "boletos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_boletos_UsuarioPersonaId",
                table: "boletos",
                column: "UsuarioPersonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_boletos_personas_UsuarioPersonaId",
                table: "boletos",
                column: "UsuarioPersonaId",
                principalTable: "personas",
                principalColumn: "PersonaId");
        }
    }
}
