using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolkminianosAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarIndicesUnicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tarifas_BairroAId_BairroBId",
                table: "Tarifas",
                columns: new[] { "BairroAId", "BairroBId" },
                unique: true);

            migrationBuilder.DropIndex(
                name: "IX_Tarifas_BairroAId",
                table: "Tarifas");

            migrationBuilder.CreateIndex(
                name: "IX_Tarifas_BairroAId_BairroBId",
                table: "Tarifas",
                columns: new[] { "BairroAId", "BairroBId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pontos_Nome",
                table: "Pontos",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bairros_Nome",
                table: "Bairros",
                column: "Nome",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Tarifas_BairroAId_BairroBId",
                table: "Tarifas");

            migrationBuilder.DropIndex(
                name: "IX_Pontos_Nome",
                table: "Pontos");

            migrationBuilder.CreateIndex(
                name: "IX_Tarifas_BairroAId",
                table: "Tarifas",
                column: "BairroAId");

            migrationBuilder.DropIndex(
                name: "IX_Tarifas_BairroAId_BairroBId",
                table: "Tarifas");
        }
    }
}
