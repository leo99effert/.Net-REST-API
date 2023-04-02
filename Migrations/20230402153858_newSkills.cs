using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _Net_REST_API.Migrations
{
    /// <inheritdoc />
    public partial class newSkills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Damage", "Name" },
                values: new object[] { 3, "Wind" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Damage", "Name" },
                values: new object[] { 5, "Ice" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Damage", "Name" },
                values: new object[] { 10, "Fire" });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Damage", "Name" },
                values: new object[] { 4, 20, "Telekinesis" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Damage", "Name" },
                values: new object[] { 30, "Fireball" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Damage", "Name" },
                values: new object[] { 20, "Frenzy" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Damage", "Name" },
                values: new object[] { 50, "Blizzard" });
        }
    }
}
