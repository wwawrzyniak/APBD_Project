using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertApi.Migrations
{
    public partial class removedClientsSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "IdClient",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "IdClient",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "IdClient",
                keyValue: 22);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "IdClient", "Email", "FirstName", "LastName", "Login", "Password", "Phone", "RefreshToken" },
                values: new object[] { 20, "ww.wp.pl", "Werka", "Werkowa", "ww", "www", "899123019", null });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "IdClient", "Email", "FirstName", "LastName", "Login", "Password", "Phone", "RefreshToken" },
                values: new object[] { 21, "wb.wp.pl", "Berka", "Berkowa", "bb", "wbduqdbiw", "899121219", null });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "IdClient", "Email", "FirstName", "LastName", "Login", "Password", "Phone", "RefreshToken" },
                values: new object[] { 22, "ws.wp.pl", "Serka", "Serkowa", "ss", "wwqcefw", "899123011", null });
        }
    }
}
