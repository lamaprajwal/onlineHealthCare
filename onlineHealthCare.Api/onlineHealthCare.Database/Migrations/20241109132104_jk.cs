using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onlineHealthCare.Database.Migrations
{
    /// <inheritdoc />
    public partial class jk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisableNewAppointments",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Doctors",
                type: "varchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");


            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Doctors",
                type: "varchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropColumn(
    name: "Name",
    table: "Doctors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Doctors",
                newName: "Name");

            migrationBuilder.AddColumn<bool>(
                name: "DisableNewAppointments",
                table: "Doctors",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
