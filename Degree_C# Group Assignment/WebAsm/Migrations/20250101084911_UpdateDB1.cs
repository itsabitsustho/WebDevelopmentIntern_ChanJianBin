using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAsm.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Vendors_VendorId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_VendorId",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "ReportId",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VendorId",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorId1",
                table: "Reports",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_VendorId1",
                table: "Reports",
                column: "VendorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Vendors_VendorId1",
                table: "Reports",
                column: "VendorId1",
                principalTable: "Vendors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Vendors_VendorId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_VendorId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "VendorId1",
                table: "Reports");

            migrationBuilder.AlterColumn<string>(
                name: "VendorId",
                table: "Reports",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_VendorId",
                table: "Reports",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Vendors_VendorId",
                table: "Reports",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id");
        }
    }
}
