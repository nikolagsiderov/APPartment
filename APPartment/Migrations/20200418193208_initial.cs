using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APPartment.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<long>(nullable: false),
                    TableName = table.Column<string>(nullable: true),
                    When = table.Column<DateTime>(nullable: false),
                    KeyValues = table.Column<string>(nullable: true),
                    OldValues = table.Column<string>(nullable: true),
                    NewValues = table.Column<string>(nullable: true),
                    HouseId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    TargetObjectId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chore",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    HouseId = table.Column<long>(nullable: true),
                    ObjectId = table.Column<long>(nullable: false),
                    AssignedToId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<long>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    TargetId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "House",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    ConfirmPassword = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_House", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HouseSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<long>(nullable: false),
                    RentDueDateDay = table.Column<int>(nullable: false),
                    HouseName = table.Column<string>(nullable: true),
                    HouseId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HouseStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    HouseId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HouseUser",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hygiene",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    HouseId = table.Column<long>(nullable: true),
                    ObjectId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hygiene", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FileSize = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    TargetId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    HouseId = table.Column<long>(nullable: true),
                    ObjectId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Issue",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    HouseId = table.Column<long>(nullable: true),
                    ObjectId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<long>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    HouseId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Object",
                columns: table => new
                {
                    ObjectId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<long>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ObjectTypeId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Object", x => x.ObjectId);
                });

            migrationBuilder.CreateTable(
                name: "ObjectType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Survey",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    HouseId = table.Column<long>(nullable: true),
                    ObjectId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Survey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<long>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    ConfirmPassword = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "ObjectType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "User" },
                    { 2L, "House" },
                    { 3L, "HouseStatus" },
                    { 4L, "HouseSettings" },
                    { 5L, "Inventory" },
                    { 6L, "Hygiene" },
                    { 7L, "Issue" },
                    { 8L, "Message" },
                    { 9L, "Comment" },
                    { 10L, "Image" },
                    { 12L, "Survey" },
                    { 13L, "Chore" },
                    { 14L, "HouseUser" },
                    { 15L, "Audit" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "Chore");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "House");

            migrationBuilder.DropTable(
                name: "HouseSettings");

            migrationBuilder.DropTable(
                name: "HouseStatus");

            migrationBuilder.DropTable(
                name: "HouseUser");

            migrationBuilder.DropTable(
                name: "Hygiene");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Issue");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "Object");

            migrationBuilder.DropTable(
                name: "ObjectType");

            migrationBuilder.DropTable(
                name: "Survey");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
