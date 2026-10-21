using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketManage.Migrations
{
    /// <inheritdoc />
    public partial class SaveEnumAsStringSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .AlterColumn<string>(
                    name: "Status",
                    table: "Tickets",
                    type: "longtext",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "int"
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .AlterColumn<string>(
                    name: "Purpose",
                    table: "FileAssets",
                    type: "longtext",
                    nullable: false,
                    oldClrType: typeof(int),
                    oldType: "int"
                )
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .AlterColumn<int>(
                    name: "Status",
                    table: "Tickets",
                    type: "int",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "longtext"
                )
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .AlterColumn<int>(
                    name: "Purpose",
                    table: "FileAssets",
                    type: "int",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "longtext"
                )
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
