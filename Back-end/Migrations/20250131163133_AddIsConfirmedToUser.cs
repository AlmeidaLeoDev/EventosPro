using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventosPro.Migrations
{
    /// <inheritdoc />
    public partial class AddIsConfirmedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invites_EventId",
                table: "Invites");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invites_EventId_InvitedUserId",
                table: "Invites",
                columns: new[] { "EventId", "InvitedUserId" },
                unique: true,
                filter: "[EventId] IS NOT NULL AND [InvitedUserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Invites_EventId_InvitedUserId",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_EventId",
                table: "Invites",
                column: "EventId");
        }
    }
}
