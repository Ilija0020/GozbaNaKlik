using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gozba_na_klik_backend.Infrastructure.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class BackfillMissingRestaurantMenus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                INSERT INTO "Menus" ("RestaurantId")
                SELECT r."Id"
                FROM "Restaurants" AS r
                WHERE NOT EXISTS
                (
                    SELECT 1
                    FROM "Menus" AS m
                    WHERE m."RestaurantId" = r."Id"
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
