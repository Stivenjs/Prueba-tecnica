using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SegurosAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Insureds",
                columns: table => new
                {
                    IdentificationNumber = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FirstLastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SecondLastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ContactPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstimatedRequestValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Observations = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Insureds", x => x.IdentificationNumber);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Insureds_Email",
                table: "Insureds",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Insureds_IdentificationNumber",
                table: "Insureds",
                column: "IdentificationNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Insureds");
        }
    }
}
