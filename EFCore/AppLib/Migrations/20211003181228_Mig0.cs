using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.AppLib.Migrations
{
    public partial class Mig0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Iller",
                columns: table => new
                {
                    IlKod = table.Column<string>(type: "TEXT", nullable: false),
                    Il = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iller", x => x.IlKod);
                });

            migrationBuilder.CreateTable(
                name: "Ilceler",
                columns: table => new
                {
                    IlceKod = table.Column<string>(type: "TEXT", nullable: false),
                    Ilce = table.Column<string>(type: "TEXT", nullable: true),
                    IlKod = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ilceler", x => x.IlceKod);
                    table.ForeignKey(
                        name: "FK_Ilceler_Iller_IlKod",
                        column: x => x.IlKod,
                        principalTable: "Iller",
                        principalColumn: "IlKod",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SemtBucakBeldeler",
                columns: table => new
                {
                    SbbKod = table.Column<string>(type: "TEXT", nullable: false),
                    SemtBucakBelde = table.Column<string>(type: "TEXT", nullable: true),
                    IlceKod = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemtBucakBeldeler", x => x.SbbKod);
                    table.ForeignKey(
                        name: "FK_SemtBucakBeldeler_Ilceler_IlceKod",
                        column: x => x.IlceKod,
                        principalTable: "Ilceler",
                        principalColumn: "IlceKod",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mahalleler",
                columns: table => new
                {
                    MahalleKod = table.Column<string>(type: "TEXT", nullable: false),
                    Mahalle = table.Column<string>(type: "TEXT", nullable: true),
                    SbbKod = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahalleler", x => x.MahalleKod);
                    table.ForeignKey(
                        name: "FK_Mahalleler_SemtBucakBeldeler_SbbKod",
                        column: x => x.SbbKod,
                        principalTable: "SemtBucakBeldeler",
                        principalColumn: "SbbKod",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ilceler_IlKod",
                table: "Ilceler",
                column: "IlKod");

            migrationBuilder.CreateIndex(
                name: "IX_Mahalleler_SbbKod",
                table: "Mahalleler",
                column: "SbbKod");

            migrationBuilder.CreateIndex(
                name: "IX_SemtBucakBeldeler_IlceKod",
                table: "SemtBucakBeldeler",
                column: "IlceKod");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mahalleler");

            migrationBuilder.DropTable(
                name: "SemtBucakBeldeler");

            migrationBuilder.DropTable(
                name: "Ilceler");

            migrationBuilder.DropTable(
                name: "Iller");
        }
    }
}
