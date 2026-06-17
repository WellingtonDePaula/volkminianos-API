using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolkminianosAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameActiveToAtivoInUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bairros_Bairros_BairroId",
                table: "Bairros");

            migrationBuilder.DropForeignKey(
                name: "FK_Pontos_Bairros_BairroId",
                table: "Pontos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarifas_Bairros_BairroAId",
                table: "Tarifas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarifas_Bairros_BairroBId",
                table: "Tarifas");

            migrationBuilder.DropIndex(
                name: "IX_Bairros_BairroId",
                table: "Bairros");

            migrationBuilder.DropColumn(
                name: "BairroId",
                table: "Bairros");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Usuarios",
                newName: "Ativo");

            migrationBuilder.AddForeignKey(
                name: "FK_Pontos_Bairros_BairroId",
                table: "Pontos",
                column: "BairroId",
                principalTable: "Bairros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarifas_Bairros_BairroAId",
                table: "Tarifas",
                column: "BairroAId",
                principalTable: "Bairros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarifas_Bairros_BairroBId",
                table: "Tarifas",
                column: "BairroBId",
                principalTable: "Bairros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pontos_Bairros_BairroId",
                table: "Pontos");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarifas_Bairros_BairroAId",
                table: "Tarifas");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarifas_Bairros_BairroBId",
                table: "Tarifas");

            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "Usuarios",
                newName: "Active");

            migrationBuilder.AddColumn<int>(
                name: "BairroId",
                table: "Bairros",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bairros_BairroId",
                table: "Bairros",
                column: "BairroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bairros_Bairros_BairroId",
                table: "Bairros",
                column: "BairroId",
                principalTable: "Bairros",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pontos_Bairros_BairroId",
                table: "Pontos",
                column: "BairroId",
                principalTable: "Bairros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarifas_Bairros_BairroAId",
                table: "Tarifas",
                column: "BairroAId",
                principalTable: "Bairros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarifas_Bairros_BairroBId",
                table: "Tarifas",
                column: "BairroBId",
                principalTable: "Bairros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
