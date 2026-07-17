using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gozba_na_klik_backend.Infrastructure.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkingHoursEndsNextDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EndsNextDay",
                table: "WorkingHours",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndsNextDay",
                table: "WorkingHours");
        }
    }
}
