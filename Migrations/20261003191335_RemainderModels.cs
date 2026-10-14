using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ticketManage.Migrations
{
    /// <inheritdoc />
    public partial class RemainderModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .CreateTable(
                    name: "Conversations",
                    columns: table => new
                    {
                        Id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "MySql:ValueGenerationStrategy",
                                MySqlValueGenerationStrategy.IdentityColumn
                            ),
                        CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        CreatedBy = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ModifiedBy = table
                            .Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Conversations", x => x.Id);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "StorageObjects",
                    columns: table => new
                    {
                        Id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "MySql:ValueGenerationStrategy",
                                MySqlValueGenerationStrategy.IdentityColumn
                            ),
                        StorageProvider = table.Column<int>(type: "int", nullable: false),
                        StorageKey = table
                            .Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Data = table.Column<byte[]>(type: "longblob", nullable: true),
                        IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        CreatedBy = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ModifiedBy = table
                            .Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_StorageObjects", x => x.Id);
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "Tickets",
                    columns: table => new
                    {
                        Id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "MySql:ValueGenerationStrategy",
                                MySqlValueGenerationStrategy.IdentityColumn
                            ),
                        Status = table.Column<int>(type: "int", nullable: false),
                        IssuedByUserId = table
                            .Column<string>(type: "varchar(255)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Expiry = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                        Content = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AssignedToUserId = table
                            .Column<string>(type: "varchar(255)", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        CreatedBy = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ModifiedBy = table
                            .Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Tickets", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Tickets_AspNetUsers_AssignedToUserId",
                            column: x => x.AssignedToUserId,
                            principalTable: "AspNetUsers",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.SetNull
                        );
                        table.ForeignKey(
                            name: "FK_Tickets_AspNetUsers_IssuedByUserId",
                            column: x => x.IssuedByUserId,
                            principalTable: "AspNetUsers",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "ConversationMembers",
                    columns: table => new
                    {
                        MemberId = table
                            .Column<string>(type: "varchar(255)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ConversationId = table.Column<long>(type: "bigint", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        CreatedBy = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ModifiedBy = table
                            .Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey(
                            "PK_ConversationMembers",
                            x => new { x.MemberId, x.ConversationId }
                        );
                        table.ForeignKey(
                            name: "FK_ConversationMembers_AspNetUsers_MemberId",
                            column: x => x.MemberId,
                            principalTable: "AspNetUsers",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                        table.ForeignKey(
                            name: "FK_ConversationMembers_Conversations_ConversationId",
                            column: x => x.ConversationId,
                            principalTable: "Conversations",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "Comments",
                    columns: table => new
                    {
                        Id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "MySql:ValueGenerationStrategy",
                                MySqlValueGenerationStrategy.IdentityColumn
                            ),
                        TicketId = table.Column<long>(type: "bigint", nullable: false),
                        ParentCommentId = table.Column<long>(type: "bigint", nullable: true),
                        IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        CreatedBy = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ModifiedBy = table
                            .Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Comments", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Comments_Comments_ParentCommentId",
                            column: x => x.ParentCommentId,
                            principalTable: "Comments",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                        table.ForeignKey(
                            name: "FK_Comments_Tickets_TicketId",
                            column: x => x.TicketId,
                            principalTable: "Tickets",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "FileAssets",
                    columns: table => new
                    {
                        Id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "MySql:ValueGenerationStrategy",
                                MySqlValueGenerationStrategy.IdentityColumn
                            ),
                        FileName = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ContentType = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        Size = table.Column<long>(type: "bigint", nullable: false),
                        StorageObjectId = table.Column<long>(type: "bigint", nullable: false),
                        IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        Purpose = table.Column<int>(type: "int", nullable: false),
                        Note = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        CommentId = table.Column<long>(type: "bigint", nullable: true),
                        TicketId = table.Column<long>(type: "bigint", nullable: true),
                        CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        CreatedBy = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ModifiedBy = table
                            .Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_FileAssets", x => x.Id);
                        table.ForeignKey(
                            name: "FK_FileAssets_Comments_CommentId",
                            column: x => x.CommentId,
                            principalTable: "Comments",
                            principalColumn: "Id"
                        );
                        table.ForeignKey(
                            name: "FK_FileAssets_StorageObjects_StorageObjectId",
                            column: x => x.StorageObjectId,
                            principalTable: "StorageObjects",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade
                        );
                        table.ForeignKey(
                            name: "FK_FileAssets_Tickets_TicketId",
                            column: x => x.TicketId,
                            principalTable: "Tickets",
                            principalColumn: "Id"
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder
                .CreateTable(
                    name: "Chats",
                    columns: table => new
                    {
                        Id = table
                            .Column<long>(type: "bigint", nullable: false)
                            .Annotation(
                                "MySql:ValueGenerationStrategy",
                                MySqlValueGenerationStrategy.IdentityColumn
                            ),
                        Content = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        AttachmentId = table.Column<long>(type: "bigint", nullable: true),
                        FromUserId = table
                            .Column<string>(type: "varchar(255)", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                        CreatedBy = table
                            .Column<string>(type: "longtext", nullable: false)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                        ModifiedBy = table
                            .Column<string>(type: "longtext", nullable: true)
                            .Annotation("MySql:CharSet", "utf8mb4"),
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Chats", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Chats_AspNetUsers_FromUserId",
                            column: x => x.FromUserId,
                            principalTable: "AspNetUsers",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict
                        );
                        table.ForeignKey(
                            name: "FK_Chats_FileAssets_AttachmentId",
                            column: x => x.AttachmentId,
                            principalTable: "FileAssets",
                            principalColumn: "Id"
                        );
                    }
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_AttachmentId",
                table: "Chats",
                column: "AttachmentId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Chats_FromUserId",
                table: "Chats",
                column: "FromUserId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TicketId",
                table: "Comments",
                column: "TicketId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMembers_ConversationId",
                table: "ConversationMembers",
                column: "ConversationId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_FileAssets_CommentId",
                table: "FileAssets",
                column: "CommentId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_FileAssets_StorageObjectId",
                table: "FileAssets",
                column: "StorageObjectId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_FileAssets_TicketId",
                table: "FileAssets",
                column: "TicketId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedToUserId",
                table: "Tickets",
                column: "AssignedToUserId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IssuedByUserId",
                table: "Tickets",
                column: "IssuedByUserId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Chats");

            migrationBuilder.DropTable(name: "ConversationMembers");

            migrationBuilder.DropTable(name: "FileAssets");

            migrationBuilder.DropTable(name: "Conversations");

            migrationBuilder.DropTable(name: "Comments");

            migrationBuilder.DropTable(name: "StorageObjects");

            migrationBuilder.DropTable(name: "Tickets");
        }
    }
}
