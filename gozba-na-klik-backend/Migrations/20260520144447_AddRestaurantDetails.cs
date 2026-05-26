using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gozba_na_klik_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Restaurants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Restaurants",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "Description", "Photo" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "Description", "Photo" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "Description", "Photo" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "Description", "Photo" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "Description", "Photo" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Restaurants",
                keyColumn: "Id",
                keyValue: 106,
                columns: new[] { "Description", "Photo" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Restaurants");
        }
    }
}
