using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecoderApi.Migrations
{
    /// <inheritdoc />
    public partial class AddJsonbForDiffModifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedBytes",
                table: "DiffModifications");

            migrationBuilder.DropColumn(
                name: "OriginalBytes",
                table: "DiffModifications");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedDataJson",
                table: "DiffModifications",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalDataJson",
                table: "DiffModifications",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedDataJson",
                table: "DiffModifications");

            migrationBuilder.DropColumn(
                name: "OriginalDataJson",
                table: "DiffModifications");

            migrationBuilder.AddColumn<byte[]>(
                name: "ModifiedBytes",
                table: "DiffModifications",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "OriginalBytes",
                table: "DiffModifications",
                type: "bytea",
                nullable: true);
        }
    }
}
