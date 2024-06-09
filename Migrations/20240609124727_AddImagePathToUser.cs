using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppSummerSchool.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "UserObject",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "UserObject");
        }
    }
}
