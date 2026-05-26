using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace gozba_na_klik_backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedDummyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "IsSuspended", "Name", "Password", "Photo", "Role", "Surname", "Username", "WorkplaceRestaurantId" },
                values: new object[,]
                {
                    { 101, "petar@vlasnik.com", false, false, "Petar", "123", null, "Owner", "Petrovic", "petar_v", null },
                    { 102, "marko@vlasnik.com", false, false, "Marko", "123", null, "Owner", "Markovic", "marko_v", null },
                    { 103, "jovan@vlasnik.com", false, false, "Jovan", "123", null, "Owner", "Jovanovic", "jovan_v", null }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "Address", "IsDeleted", "Name", "OwnerId" },
                values: new object[,]
                {
                    { 101, "Glavna 10, Novi Sad", false, "Restoran Kod Petra", 101 },
                    { 102, "Dunavska 5, Novi Sad", false, "Petrova Picerija", 101 },
                    { 103, "Zmaj Jovina 12, Novi Sad", false, "Markova Kafana", 102 },
                    { 104, "Bulevar Oslobodjenja 45, Novi Sad", false, "Marko Grill", 102 },
                    { 105, "Laze Teleckog 3, Novi Sad", false, "Jovanov Restoran", 103 },
                    { 106, "Futoski put 10, Novi Sad", false, "Pekara Jovan", 103 }
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "RestaurantId" },
                values: new object[,]
                {
                    { 101, 101 },
                    { 102, 102 },
                    { 103, 103 },
                    { 104, 104 },
                    { 105, 105 },
                    { 106, 106 }
                });

            migrationBuilder.InsertData(
                table: "Meals",
                columns: new[] { "Id", "Description", "IsDeleted", "MenuId", "Name", "Photo", "Price" },
                values: new object[,]
                {
                    { 101, "10 komada sa lukom", false, 101, "Cevapi", null, 800m },
                    { 102, "Gurmanska 250g", false, 101, "Pljeskavica", null, 700m },
                    { 103, "Paradajz, krastavac, sir", false, 101, "Sopska Salata", null, 350m },
                    { 104, "Domaca corba", false, 101, "Teleca Corba", null, 400m },
                    { 105, "Sunka, sir, pecurke", false, 102, "Kapricoza", null, 900m },
                    { 106, "Pelat, sir, masline", false, 102, "Margarita", null, 750m },
                    { 107, "Kulen, ljuta paprika", false, 102, "Madjarica", null, 950m },
                    { 108, "Beli luk, maslinovo ulje", false, 102, "Pica Hleb", null, 250m },
                    { 109, "Pohovano meso, kajmak", false, 103, "Karadjordjeva", null, 1200m },
                    { 110, "Kupus, suvo meso", false, 103, "Svadbarski Kupus", null, 850m },
                    { 111, "Uz tartar sos", false, 103, "Pohovani Kackavalj", null, 600m },
                    { 112, "Topla pecena", false, 103, "Domaca Pogaca", null, 200m },
                    { 113, "Otkoštani batak 300g", false, 104, "Batak na zaru", null, 650m },
                    { 114, "Piletina na zaru", false, 104, "Belo Meso", null, 600m },
                    { 115, "Domace kobasice 300g", false, 104, "Kobasice", null, 750m },
                    { 116, "Sveze przen", false, 104, "Pomfrit", null, 250m },
                    { 117, "Govedji gulas", false, 105, "Gulas", null, 900m },
                    { 118, "Kao prilog", false, 105, "Pire Krompir", null, 200m },
                    { 119, "U susamu", false, 105, "Pohovana Piletina", null, 700m },
                    { 120, "Domaci desert", false, 105, "Krempita", null, 300m },
                    { 121, "250g", false, 106, "Burek sa Mesom", null, 220m },
                    { 122, "250g", false, 106, "Burek sa Sirom", null, 200m },
                    { 123, "Mali kroasan", false, 106, "Kroasan Cokolada", null, 120m },
                    { 124, "Casa 0.25l", false, 106, "Jogurt", null, 60m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 103);
        }
    }
}
