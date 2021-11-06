using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.AppLib.CodeFirst.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Iller",
                columns: table => new
                {
                    IlKodu = table.Column<string>(type: "char(2)", maxLength: 2, nullable: false),
                    IlAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iller", x => x.IlKodu);
                });

            migrationBuilder.CreateTable(
                name: "InternalLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InnerExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ilceler",
                columns: table => new
                {
                    IlceKodu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IlceAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IlKodu = table.Column<string>(type: "char(2)", nullable: true),
                    RowTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ilceler", x => x.IlceKodu);
                    table.ForeignKey(
                        name: "FK_Ilceler_Iller_IlKodu",
                        column: x => x.IlKodu,
                        principalTable: "Iller",
                        principalColumn: "IlKodu",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SemtBucakBeldeler",
                columns: table => new
                {
                    SbbKodu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SemtBucakBeldeAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IlceKodu = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RowTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemtBucakBeldeler", x => x.SbbKodu);
                    table.ForeignKey(
                        name: "FK_SemtBucakBeldeler_Ilceler_IlceKodu",
                        column: x => x.IlceKodu,
                        principalTable: "Ilceler",
                        principalColumn: "IlceKodu",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mahalleler",
                columns: table => new
                {
                    MahalleKodu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MahalleAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SbbKodu = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RowTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahalleler", x => x.MahalleKodu);
                    table.ForeignKey(
                        name: "FK_Mahalleler_SemtBucakBeldeler_SbbKodu",
                        column: x => x.SbbKodu,
                        principalTable: "SemtBucakBeldeler",
                        principalColumn: "SbbKodu",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ilceler_IlKodu",
                table: "Ilceler",
                column: "IlKodu");

            migrationBuilder.CreateIndex(
                name: "IX_Mahalleler_SbbKodu",
                table: "Mahalleler",
                column: "SbbKodu");

            migrationBuilder.CreateIndex(
                name: "IX_SemtBucakBeldeler_IlceKodu",
                table: "SemtBucakBeldeler",
                column: "IlceKodu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalLogs");

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
