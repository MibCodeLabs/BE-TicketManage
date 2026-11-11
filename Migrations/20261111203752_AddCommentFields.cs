using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketManage.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .AddColumn<string>(
                    name: "Content",
                    table: "Comments",
                    type: "longtext",
                    nullable: false
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .AddColumn<string>(
                    name: "CreatedByUserId",
                    table: "Comments",
                    type: "varchar(255)",
                    nullable: false,
                    defaultValue: ""
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated",
                table: "Comments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedByUserId",
                table: "Comments",
                column: "CreatedByUserId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedByUserId",
                table: "Comments",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedByUserId",
                table: "Comments"
            );

            migrationBuilder.DropIndex(name: "IX_Comments_CreatedByUserId", table: "Comments");

            migrationBuilder.DropColumn(name: "Content", table: "Comments");

            migrationBuilder.DropColumn(name: "CreatedByUserId", table: "Comments");

            migrationBuilder.DropColumn(name: "IsUpdated", table: "Comments");
        }
    }
}
