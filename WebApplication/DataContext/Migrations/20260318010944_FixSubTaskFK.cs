using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace Repository.Migrations
{
    public partial class FixSubTaskFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_Users_Id",
                table: "SubTasks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}