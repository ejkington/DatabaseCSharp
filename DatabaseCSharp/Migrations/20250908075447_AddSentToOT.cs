using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseCSharp.Migrations
{
    /// <inheritdoc />
    public partial class AddSentToOT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SentToOT",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentToOT",
                table: "Orders");
        }
    }
}
