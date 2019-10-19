using Microsoft.EntityFrameworkCore.Migrations;

namespace APPartment.Server.Migrations
{
    public partial class UserHouseID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HouseId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HouseId",
                table: "AspNetUsers");
        }
    }
}
