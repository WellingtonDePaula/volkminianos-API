using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolkminianosAPI.Migrations {
    /// <inheritdoc />
    public partial class AdicionarIndicesUnicos : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            // O MySQL exige um índice cobrindo a chave estrangeira de BairroAId.
            // O índice composto (BairroAId, BairroBId) faz esse papel, então ele
            // precisa ser criado ANTES de apagar o índice simples.
            migrationBuilder.CreateIndex(
                name: "IX_Tarifas_BairroAId_BairroBId",
                table: "Tarifas",
                columns: new[] { "BairroAId", "BairroBId" },
                unique: true);

            migrationBuilder.DropIndex(
                name: "IX_Tarifas_BairroAId",
                table: "Tarifas");

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

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            // Ordem inversa: recria o índice simples ANTES de apagar o composto.
            migrationBuilder.CreateIndex(
                name: "IX_Tarifas_BairroAId",
                table: "Tarifas",
                column: "BairroAId");

            migrationBuilder.DropIndex(
                name: "IX_Tarifas_BairroAId_BairroBId",
                table: "Tarifas");

            migrationBuilder.DropIndex(
                name: "IX_Pontos_Nome",
                table: "Pontos");

            migrationBuilder.DropIndex(
                name: "IX_Bairros_Nome",
                table: "Bairros");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");
        }
    }
}