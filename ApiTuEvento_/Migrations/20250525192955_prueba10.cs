using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTuEvento_.Migrations
{
    /// <inheritdoc />
    public partial class prueba10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "eventos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen",
                table: "eventos",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
