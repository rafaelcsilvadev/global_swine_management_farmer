using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalSwineManagementFarmer.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthAndSymptoms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthMedications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Media = table.Column<string>(type: "text", nullable: true),
                    TreatmentEfficacy = table.Column<string>(type: "text", nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthMedications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthMedications_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HealthMedications_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Symptoms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Symptom = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symptoms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SymptomsObserved",
                columns: table => new
                {
                    SymptomId = table.Column<Guid>(type: "uuid", nullable: false),
                    HealthMedicationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomsObserved", x => new { x.SymptomId, x.HealthMedicationId });
                    table.ForeignKey(
                        name: "FK_SymptomsObserved_HealthMedications_HealthMedicationId",
                        column: x => x.HealthMedicationId,
                        principalTable: "HealthMedications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SymptomsObserved_Symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthMedications_BatchId",
                table: "HealthMedications",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthMedications_WarehouseId",
                table: "HealthMedications",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomsObserved_HealthMedicationId",
                table: "SymptomsObserved",
                column: "HealthMedicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SymptomsObserved");

            migrationBuilder.DropTable(
                name: "HealthMedications");

            migrationBuilder.DropTable(
                name: "Symptoms");
        }
    }
}
