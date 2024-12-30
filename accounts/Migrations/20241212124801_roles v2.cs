using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace accounts.Migrations
{
    /// <inheritdoc />
    public partial class rolesv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_account_roles_role_id",
                table: "account_roles",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "fk_account_roles_roles_role_id",
                table: "account_roles",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_account_roles_roles_role_id",
                table: "account_roles");

            migrationBuilder.DropIndex(
                name: "ix_account_roles_role_id",
                table: "account_roles");
        }
    }
}
