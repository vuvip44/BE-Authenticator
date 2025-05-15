using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Login.api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DiemCC",
                table: "Students",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DiemCuoiKy",
                table: "Students",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DiemGiuaKy",
                table: "Students",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DiemTongKet",
                table: "Students",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "XepLoai",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiemCC",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DiemCuoiKy",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DiemGiuaKy",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DiemTongKet",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "XepLoai",
                table: "Students");
        }
    }
}
