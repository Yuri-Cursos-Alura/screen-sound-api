using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScreenSound.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarGeneroMusica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneroId",
                table: "Musicas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Genero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genero", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Musicas_GeneroId",
                table: "Musicas",
                column: "GeneroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Musicas_Genero_GeneroId",
                table: "Musicas",
                column: "GeneroId",
                principalTable: "Genero",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Musicas_Genero_GeneroId",
                table: "Musicas");

            migrationBuilder.DropTable(
                name: "Genero");

            migrationBuilder.DropIndex(
                name: "IX_Musicas_GeneroId",
                table: "Musicas");

            migrationBuilder.DropColumn(
                name: "GeneroId",
                table: "Musicas");
        }
    }
}
