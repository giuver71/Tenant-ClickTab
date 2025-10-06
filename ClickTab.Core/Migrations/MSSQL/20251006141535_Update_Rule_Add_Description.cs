using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClickTab.Core.Migrations.MSSQL
{
    /// <inheritdoc />
    public partial class Update_Rule_Add_Description : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleRule_Roles_RoleID",
                table: "RoleRule");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleRule_Rule_RuleID",
                table: "RoleRule");

            migrationBuilder.DropForeignKey(
                name: "FK_UserARoles_Roles_FK_Role",
                table: "UserARoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserARoles_Users_FK_User",
                table: "UserARoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserARoles",
                table: "UserARoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rule",
                table: "Rule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleRule",
                table: "RoleRule");

            migrationBuilder.DropColumn(
                name: "DescriptionEnum",
                table: "Rule");

            migrationBuilder.RenameTable(
                name: "UserARoles",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "Rule",
                newName: "Rules");

            migrationBuilder.RenameTable(
                name: "RoleRule",
                newName: "RoleRules");

            migrationBuilder.RenameIndex(
                name: "IX_UserARoles_FK_User",
                table: "UserRoles",
                newName: "IX_UserRoles_FK_User");

            migrationBuilder.RenameIndex(
                name: "IX_UserARoles_FK_Role",
                table: "UserRoles",
                newName: "IX_UserRoles_FK_Role");

            migrationBuilder.RenameIndex(
                name: "IX_RoleRule_RuleID",
                table: "RoleRules",
                newName: "IX_RoleRules_RuleID");

            migrationBuilder.RenameIndex(
                name: "IX_RoleRule_RoleID",
                table: "RoleRules",
                newName: "IX_RoleRules_RoleID");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rules",
                table: "Rules",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleRules",
                table: "RoleRules",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleRules_Roles_RoleID",
                table: "RoleRules",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleRules_Rules_RuleID",
                table: "RoleRules",
                column: "RuleID",
                principalTable: "Rules",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_FK_Role",
                table: "UserRoles",
                column: "FK_Role",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_FK_User",
                table: "UserRoles",
                column: "FK_User",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleRules_Roles_RoleID",
                table: "RoleRules");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleRules_Rules_RuleID",
                table: "RoleRules");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_FK_Role",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_FK_User",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rules",
                table: "Rules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleRules",
                table: "RoleRules");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rules");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserARoles");

            migrationBuilder.RenameTable(
                name: "Rules",
                newName: "Rule");

            migrationBuilder.RenameTable(
                name: "RoleRules",
                newName: "RoleRule");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_FK_User",
                table: "UserARoles",
                newName: "IX_UserARoles_FK_User");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_FK_Role",
                table: "UserARoles",
                newName: "IX_UserARoles_FK_Role");

            migrationBuilder.RenameIndex(
                name: "IX_RoleRules_RuleID",
                table: "RoleRule",
                newName: "IX_RoleRule_RuleID");

            migrationBuilder.RenameIndex(
                name: "IX_RoleRules_RoleID",
                table: "RoleRule",
                newName: "IX_RoleRule_RoleID");

            migrationBuilder.AddColumn<int>(
                name: "DescriptionEnum",
                table: "Rule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserARoles",
                table: "UserARoles",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rule",
                table: "Rule",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleRule",
                table: "RoleRule",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleRule_Roles_RoleID",
                table: "RoleRule",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleRule_Rule_RuleID",
                table: "RoleRule",
                column: "RuleID",
                principalTable: "Rule",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserARoles_Roles_FK_Role",
                table: "UserARoles",
                column: "FK_Role",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserARoles_Users_FK_User",
                table: "UserARoles",
                column: "FK_User",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
