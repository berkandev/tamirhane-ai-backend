using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecoderApi.Migrations
{
    /// <summary>
    /// DiffModification tablosuna JSONB desteği ekleyen migration
    /// </summary>
    public partial class AddJsonbForDiffModifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Önce eski sütunları kaldır (eğer varsa)
            migrationBuilder.DropColumn(
                name: "OriginalBytes",
                table: "DiffModifications");

            migrationBuilder.DropColumn(
                name: "ModifiedBytes",
                table: "DiffModifications");

            // Yeni JSONB sütunları ekle
            migrationBuilder.AddColumn<string>(
                name: "OriginalDataJson",
                table: "DiffModifications",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedDataJson",
                table: "DiffModifications",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri alma durumunda JSONB sütunları kaldır
            migrationBuilder.DropColumn(
                name: "OriginalDataJson",
                table: "DiffModifications");

            migrationBuilder.DropColumn(
                name: "ModifiedDataJson",
                table: "DiffModifications");

            // Orijinal byte array sütunları geri ekle
            migrationBuilder.AddColumn<byte[]>(
                name: "OriginalBytes",
                table: "DiffModifications",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ModifiedBytes",
                table: "DiffModifications",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}