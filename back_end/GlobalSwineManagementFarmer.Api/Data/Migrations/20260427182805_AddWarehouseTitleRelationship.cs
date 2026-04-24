using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalSwineManagementFarmer.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseTitleRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TitleId",
                table: "Warehouses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "WarehouseTitles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseTitles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_TitleId",
                table: "Warehouses",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTitles_Title",
                table: "WarehouseTitles",
                column: "Title",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_WarehouseTitles_TitleId",
                table: "Warehouses",
                column: "TitleId",
                principalTable: "WarehouseTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_WarehouseTitles_TitleId",
                table: "Warehouses");

            migrationBuilder.DropTable(
                name: "WarehouseTitles");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_TitleId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "Warehouses");
        }
    }
}
