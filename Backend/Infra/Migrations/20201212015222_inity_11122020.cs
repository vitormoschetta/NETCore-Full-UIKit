using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infra.Migrations
{
    public partial class inity_11122020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Acao = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    Data = table.Column<DateTime>(type: "datetime", nullable: false),
                    Usuario = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    TabelaModificada = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true),
                    Dados = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAuth",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    Password = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Salt = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Role = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuth", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessLog");

            migrationBuilder.DropTable(
                name: "UserAuth");
        }
    }
}
