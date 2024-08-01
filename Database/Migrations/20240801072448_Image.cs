using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class Image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    file_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_path = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    upload_date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 7, 24, 47, 851, DateTimeKind.Utc).AddTicks(1427), new DateTime(2024, 8, 1, 7, 24, 47, 851, DateTimeKind.Utc).AddTicks(1429) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 7, 24, 47, 851, DateTimeKind.Utc).AddTicks(1432), new DateTime(2024, 8, 1, 7, 24, 47, 851, DateTimeKind.Utc).AddTicks(1432) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: "1",
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 15, 24, 47, 851, DateTimeKind.Local).AddTicks(4825), new DateTime(2024, 8, 1, 15, 24, 47, 851, DateTimeKind.Local).AddTicks(4835) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: "2",
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 15, 24, 47, 851, DateTimeKind.Local).AddTicks(4838), new DateTime(2024, 8, 1, 15, 24, 47, 851, DateTimeKind.Local).AddTicks(4838) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: "3",
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 15, 24, 47, 851, DateTimeKind.Local).AddTicks(4839), new DateTime(2024, 8, 1, 15, 24, 47, 851, DateTimeKind.Local).AddTicks(4840) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: "4",
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 15, 24, 47, 851, DateTimeKind.Local).AddTicks(4841), new DateTime(2024, 8, 1, 15, 24, 47, 851, DateTimeKind.Local).AddTicks(4841) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 6, 27, 48, 168, DateTimeKind.Utc).AddTicks(198), new DateTime(2024, 8, 1, 6, 27, 48, 168, DateTimeKind.Utc).AddTicks(200) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 6, 27, 48, 168, DateTimeKind.Utc).AddTicks(204), new DateTime(2024, 8, 1, 6, 27, 48, 168, DateTimeKind.Utc).AddTicks(204) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: "1",
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 14, 27, 48, 168, DateTimeKind.Local).AddTicks(3049), new DateTime(2024, 8, 1, 14, 27, 48, 168, DateTimeKind.Local).AddTicks(3057) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: "2",
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 14, 27, 48, 168, DateTimeKind.Local).AddTicks(3058), new DateTime(2024, 8, 1, 14, 27, 48, 168, DateTimeKind.Local).AddTicks(3059) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: "3",
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 14, 27, 48, 168, DateTimeKind.Local).AddTicks(3060), new DateTime(2024, 8, 1, 14, 27, 48, 168, DateTimeKind.Local).AddTicks(3060) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: "4",
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2024, 8, 1, 14, 27, 48, 168, DateTimeKind.Local).AddTicks(3062), new DateTime(2024, 8, 1, 14, 27, 48, 168, DateTimeKind.Local).AddTicks(3062) });
        }
    }
}
