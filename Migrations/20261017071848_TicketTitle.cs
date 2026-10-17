using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketManage.Migrations
{
    /// <inheritdoc />
    public partial class TicketTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .AddColumn<string>(
                    name: "Title",
                    table: "Tickets",
                    type: "longtext",
                    nullable: false
                )
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Title", table: "Tickets");
        }
    }
}
