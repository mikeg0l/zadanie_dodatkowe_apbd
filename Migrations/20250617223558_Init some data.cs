using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZadanieDodatkowe.Migrations
{
    /// <inheritdoc />
    public partial class Initsomedata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Atendee",
                columns: new[] { "ID", "Email", "FirstName", "LastName" },
                values: new object[] { 1, "boris.goryl@example.com", "Boris", "Goryl" });

            migrationBuilder.InsertData(
                table: "Speaker",
                columns: new[] { "ID", "FirstName", "LastName" },
                values: new object[] { 1, "Grzegorz", "Kurka" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Atendee",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Speaker",
                keyColumn: "ID",
                keyValue: 1);
        }
    }
}
