using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolkminianosAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCampoSenhaNoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Senha",
                table: "Usuarios",
                type: "int",
                maxLength: 256,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Usuarios");
        }
    }
}
