using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAppUserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Pacientes_PacienteId",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensajerias_Pacientes_PacienteId",
                table: "Mensajerias");

            migrationBuilder.DropForeignKey(
                name: "FK_TestDetails_Pacientes_PacienteId",
                table: "TestDetails");

            migrationBuilder.DropIndex(
                name: "IX_TestDetails_PacienteId",
                table: "TestDetails");

            migrationBuilder.DropIndex(
                name: "IX_Mensajerias_PacienteId",
                table: "Mensajerias");

            migrationBuilder.DropIndex(
                name: "IX_Citas_PacienteId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "TestDetails");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "Mensajerias");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "Citas");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TestDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Mensajerias",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Info",
                table: "Especialistas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Citas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Citas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TestDetails_ApplicationUserId",
                table: "TestDetails",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajerias_ApplicationUserId",
                table: "Mensajerias",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_ApplicationUserId",
                table: "Citas",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_AspNetUsers_ApplicationUserId",
                table: "Citas",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensajerias_AspNetUsers_ApplicationUserId",
                table: "Mensajerias",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestDetails_AspNetUsers_ApplicationUserId",
                table: "TestDetails",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_AspNetUsers_ApplicationUserId",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensajerias_AspNetUsers_ApplicationUserId",
                table: "Mensajerias");

            migrationBuilder.DropForeignKey(
                name: "FK_TestDetails_AspNetUsers_ApplicationUserId",
                table: "TestDetails");

            migrationBuilder.DropIndex(
                name: "IX_TestDetails_ApplicationUserId",
                table: "TestDetails");

            migrationBuilder.DropIndex(
                name: "IX_Mensajerias_ApplicationUserId",
                table: "Mensajerias");

            migrationBuilder.DropIndex(
                name: "IX_Citas_ApplicationUserId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TestDetails");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Mensajerias");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Citas");

            migrationBuilder.AddColumn<int>(
                name: "PacienteId",
                table: "TestDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PacienteId",
                table: "Mensajerias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Info",
                table: "Especialistas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PacienteId",
                table: "Citas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TestDetails_PacienteId",
                table: "TestDetails",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajerias_PacienteId",
                table: "Mensajerias",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_PacienteId",
                table: "Citas",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Pacientes_PacienteId",
                table: "Citas",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensajerias_Pacientes_PacienteId",
                table: "Mensajerias",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestDetails_Pacientes_PacienteId",
                table: "TestDetails",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "PacienteId");
        }
    }
}
