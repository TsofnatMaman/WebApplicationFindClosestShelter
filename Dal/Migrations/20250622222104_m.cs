using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class m : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shelters",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelters", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location_Longitude = table.Column<double>(type: "float", nullable: false),
                    Location_Latitude = table.Column<double>(type: "float", nullable: false),
                    ShelterCode = table.Column<int>(type: "int", nullable: false),
                    IsOpen24_7 = table.Column<bool>(type: "bit", nullable: false),
                    ContactPersonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonHasSMS = table.Column<bool>(type: "bit", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CurrentNumberPeople = table.Column<int>(type: "int", nullable: false),
                    AddedSystem = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Addresses_Shelters_ShelterCode",
                        column: x => x.ShelterCode,
                        principalTable: "Shelters",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Opinsiones",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressCode = table.Column<int>(type: "int", nullable: false),
                    Stars = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opinsiones", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Opinsiones_Addresses_AddressCode",
                        column: x => x.AddressCode,
                        principalTable: "Addresses",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ShelterCode",
                table: "Addresses",
                column: "ShelterCode");

            migrationBuilder.CreateIndex(
                name: "IX_Opinsiones_AddressCode",
                table: "Opinsiones",
                column: "AddressCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opinsiones");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Shelters");
        }
    }
}
