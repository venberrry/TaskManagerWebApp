using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppSummerSchool.Migrations
{
    /// <inheritdoc />
    public partial class NotInitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserObjectId",
                table: "TaskObject",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskObject_UserId",
                table: "TaskObject",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskObject_UserObjectId",
                table: "TaskObject",
                column: "UserObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskObject_UserObject_UserId",
                table: "TaskObject",
                column: "UserId",
                principalTable: "UserObject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskObject_UserObject_UserObjectId",
                table: "TaskObject",
                column: "UserObjectId",
                principalTable: "UserObject",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskObject_UserObject_UserId",
                table: "TaskObject");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskObject_UserObject_UserObjectId",
                table: "TaskObject");

            migrationBuilder.DropIndex(
                name: "IX_TaskObject_UserId",
                table: "TaskObject");

            migrationBuilder.DropIndex(
                name: "IX_TaskObject_UserObjectId",
                table: "TaskObject");

            migrationBuilder.DropColumn(
                name: "UserObjectId",
                table: "TaskObject");
        }
    }
}
