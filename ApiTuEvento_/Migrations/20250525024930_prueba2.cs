using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTuEvento_.Migrations
{
    /// <inheritdoc />
    public partial class prueba2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "categoriaEventos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "categoriaEventos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
