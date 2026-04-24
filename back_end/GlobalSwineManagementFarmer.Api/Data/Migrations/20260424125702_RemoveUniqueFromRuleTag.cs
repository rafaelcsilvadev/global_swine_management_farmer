using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalSwineManagementFarmer.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueFromRuleTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rules_Tag",
                table: "Rules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rules_Tag",
                table: "Rules",
                column: "Tag",
                unique: true);
        }
    }
}
