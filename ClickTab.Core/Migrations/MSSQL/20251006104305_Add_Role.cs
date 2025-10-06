using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClickTab.Core.Migrations.MSSQL
{
    /// <inheritdoc />
    public partial class Add_Role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FK_InsertUser = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FK_UpdateUser = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rule",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescriptionEnum = table.Column<int>(type: "int", nullable: false),
                    UrlRoutes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rule", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserARoles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_User = table.Column<int>(type: "int", nullable: false),
                    FK_Role = table.Column<int>(type: "int", nullable: false),
                    FK_InsertUser = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FK_UpdateUser = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserARoles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserARoles_Roles_FK_Role",
                        column: x => x.FK_Role,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserARoles_Users_FK_User",
                        column: x => x.FK_User,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleRule",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FK_Role = table.Column<int>(type: "int", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: true),
                    FK_Rule = table.Column<int>(type: "int", nullable: false),
                    RuleID = table.Column<int>(type: "int", nullable: true),
                    CanCreate = table.Column<bool>(type: "bit", nullable: false),
                    CanEdit = table.Column<bool>(type: "bit", nullable: false),
                    CanDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleRule", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoleRule_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_RoleRule_Rule_RuleID",
                        column: x => x.RuleID,
                        principalTable: "Rule",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleRule_RoleID",
                table: "RoleRule",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_RoleRule_RuleID",
                table: "RoleRule",
                column: "RuleID");

            migrationBuilder.CreateIndex(
                name: "IX_UserARoles_FK_Role",
                table: "UserARoles",
                column: "FK_Role");

            migrationBuilder.CreateIndex(
                name: "IX_UserARoles_FK_User",
                table: "UserARoles",
                column: "FK_User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleRule");

            migrationBuilder.DropTable(
                name: "UserARoles");

            migrationBuilder.DropTable(
                name: "Rule");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
