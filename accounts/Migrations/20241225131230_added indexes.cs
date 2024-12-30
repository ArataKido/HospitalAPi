using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace accounts.Migrations
{
    /// <inheritdoc />
    public partial class addedindexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_accounts_full_name",
                table: "accounts",
                column: "full_name");

            migrationBuilder.CreateIndex(
                name: "ix_accounts_user_name",
                table: "accounts",
                column: "user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_accounts_full_name",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "ix_accounts_user_name",
                table: "accounts");
        }
    }
}
