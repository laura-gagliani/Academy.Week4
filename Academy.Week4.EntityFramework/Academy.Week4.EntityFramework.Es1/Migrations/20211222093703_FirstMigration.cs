using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academy.Week4.EntityFramework.Es1.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Azienda",
                columns: table => new
                {
                    AziendaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Azienda", x => x.AziendaID);
                });

            migrationBuilder.CreateTable(
                name: "Impiegato",
                columns: table => new
                {
                    ImpiegatoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataDiNascita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AziendaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impiegato", x => x.ImpiegatoID);
                    table.ForeignKey(
                        name: "FK_Impiegato_Azienda_AziendaID",
                        column: x => x.AziendaID,
                        principalTable: "Azienda",
                        principalColumn: "AziendaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Impiegato_AziendaID",
                table: "Impiegato",
                column: "AziendaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Impiegato");

            migrationBuilder.DropTable(
                name: "Azienda");
        }
    }
}
