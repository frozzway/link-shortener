using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkShortener.Migrations
{
    /// <inheritdoc />
    public partial class linkcounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Counter",
                table: "Links",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Counter",
                table: "Links");
        }
    }
}
