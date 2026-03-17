using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class RecreateSubTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_Tasks_TasksId",
                table: "SubTasks");

            migrationBuilder.DropIndex(
                name: "IX_SubTasks_TasksId",
                table: "SubTasks");

            migrationBuilder.DropColumn(
                name: "TasksId",
                table: "SubTasks");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_TaskId",
                table: "SubTasks",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTasks_Tasks_TaskId",
                table: "SubTasks",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_Tasks_TaskId",
                table: "SubTasks");

            migrationBuilder.DropIndex(
                name: "IX_SubTasks_TaskId",
                table: "SubTasks");

            migrationBuilder.AddColumn<int>(
                name: "TasksId",
                table: "SubTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_TasksId",
                table: "SubTasks",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTasks_Tasks_TasksId",
                table: "SubTasks",
                column: "TasksId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
