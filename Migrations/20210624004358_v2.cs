using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenMaps.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoPontoId",
                table: "PontoColeta",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TipoLixo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoLixo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoPonto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoPonto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PontoColetaTipoLixo",
                columns: table => new
                {
                    PontoColetasId = table.Column<int>(type: "int", nullable: false),
                    TipoLixosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PontoColetaTipoLixo", x => new { x.PontoColetasId, x.TipoLixosId });
                    table.ForeignKey(
                        name: "FK_PontoColetaTipoLixo_PontoColeta_PontoColetasId",
                        column: x => x.PontoColetasId,
                        principalTable: "PontoColeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PontoColetaTipoLixo_TipoLixo_TipoLixosId",
                        column: x => x.TipoLixosId,
                        principalTable: "TipoLixo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PontoColeta_TipoPontoId",
                table: "PontoColeta",
                column: "TipoPontoId");

            migrationBuilder.CreateIndex(
                name: "IX_PontoColetaTipoLixo_TipoLixosId",
                table: "PontoColetaTipoLixo",
                column: "TipoLixosId");

            migrationBuilder.AddForeignKey(
                name: "FK_PontoColeta_TipoPonto_TipoPontoId",
                table: "PontoColeta",
                column: "TipoPontoId",
                principalTable: "TipoPonto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PontoColeta_TipoPonto_TipoPontoId",
                table: "PontoColeta");

            migrationBuilder.DropTable(
                name: "PontoColetaTipoLixo");

            migrationBuilder.DropTable(
                name: "TipoPonto");

            migrationBuilder.DropTable(
                name: "TipoLixo");

            migrationBuilder.DropIndex(
                name: "IX_PontoColeta_TipoPontoId",
                table: "PontoColeta");

            migrationBuilder.DropColumn(
                name: "TipoPontoId",
                table: "PontoColeta");
        }
    }
}
