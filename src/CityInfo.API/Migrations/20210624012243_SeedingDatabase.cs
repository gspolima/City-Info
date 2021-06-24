using Microsoft.EntityFrameworkCore.Migrations;

namespace CityInfo.API.Migrations
{
    public partial class SeedingDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "A fairly well-known rock n roll band was born there.", "Liverpool" },
                    { 2, "The city with a gigantic tower.", "Paris" },
                    { 3, "The city with a cathedral that was never really finished.", "Antwerp" },
                    { 4, "A little glimpse of Britain in Spaniard lands.", "Gibraltar" }
                });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, "The world’s largest permanent exhibition purely devoted to telling the story of The Beatles’ rise to fame.", "The Beatles Story" },
                    { 2, 1, "A magnificent 200-acre Park that looks like a natural landscape rather than a man-made park.", "Sefton Park" },
                    { 3, 2, "Landmark art museum with vast collection.", "Louvre Museum" },
                    { 4, 2, "Triumphal arch and national monument.", "Arc de Triomphe" },
                    { 5, 3, "The Grote Markt with its town hall and numerous guild houses is the heart of the old town.", "Grand Place" },
                    { 6, 3, "Belgium's largest Gothic church.", "Cathedral of Our Lady" },
                    { 7, 4, "This famous limestone promontory features a nature reserve, a network of tunnels & sea views.", "Rock of Gibraltar" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
