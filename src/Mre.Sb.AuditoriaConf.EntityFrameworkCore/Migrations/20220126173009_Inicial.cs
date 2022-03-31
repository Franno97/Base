using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mre.Sb.AuditoriaConf.EntityFrameworkCore.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auditable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoriaId = table.Column<string>(type: "nvarchar(6)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Item = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditable_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auditar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Acciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditar_Auditable_AuditableId",
                        column: x => x.AuditableId,
                        principalTable: "Auditable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditable_CategoriaId",
                table: "Auditable",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditar_AuditableId",
                table: "Auditar",
                column: "AuditableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditar");

            migrationBuilder.DropTable(
                name: "Auditable");

            migrationBuilder.DropTable(
                name: "Categoria");
        }
    }
}
