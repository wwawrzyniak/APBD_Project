using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertApi.Migrations
{
    public partial class seedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Building",
                columns: new[] { "IdBuilding", "City", "Height", "Street", "StreetNumber" },
                values: new object[,]
                {
                    { 1, "Warsaw", 16m, "pretty", 1 },
                    { 2, "Warsaw", 10m, "pretty", 2 },
                    { 3, "Warsaw", 7m, "pretty", 3 },
                    { 4, "Warsaw", 12m, "pretty", 4 },
                    { 5, "Warsaw", 20m, "nice", 10 },
                    { 6, "Warsaw", 30m, "nice", 12 },
                    { 7, "Warsaw", 2m, "nice", 14 }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "IdClient", "Email", "FirstName", "LastName", "Login", "Password", "Phone", "RefreshToken" },
                values: new object[,]
                {
                    { 20, "ww.wp.pl", "Werka", "Werkowa", "ww", "www", "899123019", null },
                    { 21, "wb.wp.pl", "Berka", "Berkowa", "bb", "wbduqdbiw", "899121219", null },
                    { 22, "ws.wp.pl", "Serka", "Serkowa", "ss", "wwqcefw", "899123011", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "IdBuilding",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "IdBuilding",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "IdBuilding",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "IdBuilding",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "IdBuilding",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "IdBuilding",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Building",
                keyColumn: "IdBuilding",
                keyValue: 7);

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
    }
}
