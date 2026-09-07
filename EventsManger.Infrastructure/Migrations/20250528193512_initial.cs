using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventManger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "blogPosts",
                columns: table => new
                {
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimePosted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogPosts", x => x.BlogId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganizationID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Views = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventID);
                });

            migrationBuilder.CreateTable(
                name: "NewsArticles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsArticles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    College = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationID);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    RoomID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.RoomID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogPostBlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_blogPosts_BlogPostBlogId",
                        column: x => x.BlogPostBlogId,
                        principalTable: "blogPosts",
                        principalColumn: "BlogId");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommentatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeCommented = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_blogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "blogPosts",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserEvent",
                columns: table => new
                {
                    AttendedEventsEventID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttendeesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserEvent", x => new { x.AttendedEventsEventID, x.AttendeesId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserEvent_AspNetUsers_AttendeesId",
                        column: x => x.AttendeesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserEvent_Events_AttendedEventsEventID",
                        column: x => x.AttendedEventsEventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("45eded88-eaff-4dd7-8b4e-873f88c39ea2"), null, "Admin", "ADMIN" },
                    { new Guid("6b1e722c-0e06-4365-854d-112a34f7eb96"), null, "Attendee", "ATTENDEE" },
                    { new Guid("cfa3e7d7-dba9-472d-a964-284b842a8280"), null, "Guest", "GUEST" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BlogPostBlogId", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PersonName", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("7903876c-ada5-4e91-b9e3-a6682005d4b7"), 0, null, "dd870a8a-ed1f-46d5-889b-82573bf850e9", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEERsphduIQwSiBfNYU/u7NGqyyUlRVR68Xdcm2+XHi5aQDDiGg3wzUIT/1aWEnXWtg==", "admin", null, false, "5e2ebdd7-1db0-436f-bc37-40746c1f5577", false, "admin@example.com" },
                    { new Guid("a30856d8-e5ae-4acb-a6ba-069f6e6af117"), 0, null, "7437a040-400b-4bed-a3fc-39ab832311f5", "user2@example.com", true, false, null, "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEO4jOjeYXgHf/TVXCVnmQp6irsplcfXTsvnavluQ4j15UWgfhX28H/zfGLz7SGR+BA==", "user2", null, false, "ebffa888-2a46-42d6-962c-f381b0d98420", false, "user2@example.com" },
                    { new Guid("d1055d57-7f97-4c75-945e-46101906aa6e"), 0, null, "fe131c40-d54b-4fe1-8b51-798478679e5b", "user1@example.com", true, false, null, "USER1@EXAMPLE.COM", "USER1@EXAMPLE.COM", "AQAAAAIAAYagAAAAEBmyc7407ZYCefLg9318FId7NWEXPwR3YZu0h7vMmwsESZcCkp85BrxuLIwbrglNdg==", "user1", null, false, "61660e59-aeac-4ef1-97f8-095ef2f24fd0", false, "user1@example.com" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventID", "Description", "EndTime", "Name", "OrganizationID", "PhotoUrl", "RoomID", "StartTime", "Status", "Views" },
                values: new object[,]
                {
                    { new Guid("3c43b83d-e31d-4bb1-ae66-bf82c44826bf"), "A competition to solve real-world problems.", new DateTime(2025, 6, 12, 1, 35, 12, 225, DateTimeKind.Local).AddTicks(6117), "Problem Solving Competition", new Guid("8d68f10d-7cfe-43a6-a49d-406685b0ea0a"), "\\events-photos\\69f744c9-6ce8-4802-bf62-83b92e22babc.jpeg", new Guid("61e7f38c-9fdd-4836-8d31-2253c9ec7a49"), new DateTime(2025, 6, 11, 22, 35, 12, 225, DateTimeKind.Local).AddTicks(6117), 0, 0 },
                    { new Guid("43d2b0aa-6e63-4ce0-aa43-f68dd4f744a8"), "Monthly team meeting.", new DateTime(2025, 5, 30, 0, 35, 12, 225, DateTimeKind.Local).AddTicks(6112), "Team Meeting", new Guid("1f627b5c-7ad1-4630-b2f5-aa09feb5e4a3"), "\\events-photos\\4f289cfc-708c-4e78-b721-240be036d302.jpeg", new Guid("71571407-8325-4d99-b2c4-1922c9b552b0"), new DateTime(2025, 5, 29, 22, 35, 12, 225, DateTimeKind.Local).AddTicks(6111), 0, 0 },
                    { new Guid("9a097ff1-5b39-4325-8818-5dc75b20e948"), "Annual technology conference.", new DateTime(2025, 6, 5, 22, 35, 12, 225, DateTimeKind.Local).AddTicks(6099), "Tech Conference 2023", new Guid("cee01121-1a9b-404d-b632-23f4e12985b9"), "\\events-photos\\58147433-411f-47bc-88bb-c70b7ff2e68d.jpeg", new Guid("61e7f38c-9fdd-4836-8d31-2253c9ec7a49"), new DateTime(2025, 6, 4, 22, 35, 12, 225, DateTimeKind.Local).AddTicks(6073), 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "NewsArticles",
                columns: new[] { "Id", "Content", "CreatedDate", "PhotoUrl", "Title", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("429285e5-5729-41a3-84f4-6c0198043392"), "<p><em>Congratulations</em> to our computer science students for winning first place in the national coding competition!</p><p>The team developed an innovative <strong>AI-powered</strong> healthcare solution.</p>", new DateTime(2025, 5, 27, 22, 35, 12, 225, DateTimeKind.Local).AddTicks(6393), "\\news-photos\\1716833964113.jpg", "Student Team Wins National Competition", null },
                    { new Guid("431ae806-2e14-4317-8e1f-61e453223552"), "<p>The university has partnered with <strong>Tech Innovators</strong> to establish a new research center.</p><p><em>\"This collaboration will accelerate innovation\"</em>, said the dean of the Technology college.</p>", new DateTime(2025, 5, 18, 22, 35, 12, 225, DateTimeKind.Local).AddTicks(6284), "\\news-photos\\How-to-Build-Strategic-Partnerships-that-Boost-Sales-1.jpg", "New Research Partnership Formed", null },
                    { new Guid("b73c14e0-c5d6-4802-bf5e-2b9fb4b50f03"), "<p><strong>Exciting news!</strong> The annual Tech Conference will be held on June 04, 2025.</p><p>This year's conference will feature <em>cutting-edge</em> presentations on AI, blockchain, and quantum computing.</p>", new DateTime(2025, 5, 23, 22, 35, 12, 225, DateTimeKind.Local).AddTicks(6276), "\\news-photos\\74pCXBPyfCvb7HWEdm57HE.jpg", "Tech Conference 2023 Announced", new DateTime(2025, 5, 27, 22, 35, 12, 225, DateTimeKind.Local).AddTicks(6278) },
                    { new Guid("f13305df-25f2-4a9f-a786-a97f805c7407"), "<p>The administration has announced plans for a <strong>$20 million</strong> renovation of the main campus buildings.</p><p>Construction is expected to begin in July.</p>", new DateTime(2025, 5, 25, 22, 35, 12, 225, DateTimeKind.Local).AddTicks(6389), "\\news-photos\\BGU-NORTH-CAMPUS-02.jpg", "Campus Renovation Plans Unveiled", new DateTime(2025, 5, 26, 22, 35, 12, 225, DateTimeKind.Local).AddTicks(6390) }
                });

            migrationBuilder.InsertData(
                table: "Organizations",
                columns: new[] { "OrganizationID", "College", "ContactNumber", "Description", "Email", "LogoUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("1f627b5c-7ad1-4630-b2f5-aa09feb5e4a3"), "Medicine", null, "Providing healthcare solutions worldwide.", "info@healthsolutions.com", "\\logos\\0ce4ea06-ddf0-4b94-b3f1-e37b5e63ae3a.jpg", "Health Solutions" },
                    { new Guid("8d68f10d-7cfe-43a6-a49d-406685b0ea0a"), "Engineering", null, "Institute of Electrical and Electronics Engineers.", "IEEE@gmail.com", "\\logos\\4b210b7b-3a14-43fd-af40-752431129fb0.png", "IEEE" },
                    { new Guid("cee01121-1a9b-404d-b632-23f4e12985b9"), "Technology", null, "A leading technology innovation company.", "contact@techinnovators.com", "\\logos\\3a75a9ba-1079-4854-92c9-a17f064e54af.jpg", "Tech Innovators" }
                });

            migrationBuilder.InsertData(
                table: "rooms",
                columns: new[] { "RoomID", "Description", "Name", "Seats" },
                values: new object[,]
                {
                    { new Guid("61e7f38c-9fdd-4836-8d31-2253c9ec7a49"), "A large conference room with 50 seats.", "Conference Room A", 50 },
                    { new Guid("71571407-8325-4d99-b2c4-1922c9b552b0"), "A small meeting room with 10 seats.", "Meeting Room B", 10 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("45eded88-eaff-4dd7-8b4e-873f88c39ea2"), new Guid("7903876c-ada5-4e91-b9e3-a6682005d4b7") },
                    { new Guid("6b1e722c-0e06-4365-854d-112a34f7eb96"), new Guid("a30856d8-e5ae-4acb-a6ba-069f6e6af117") },
                    { new Guid("6b1e722c-0e06-4365-854d-112a34f7eb96"), new Guid("d1055d57-7f97-4c75-945e-46101906aa6e") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserEvent_AttendeesId",
                table: "ApplicationUserEvent",
                column: "AttendeesId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BlogPostBlogId",
                table: "AspNetUsers",
                column: "BlogPostBlogId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogPostId",
                table: "Comments",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentId",
                table: "Comments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserId",
                table: "Messages",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserEvent");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "NewsArticles");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "blogPosts");
        }
    }
}
