using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventService.Data.migrations
{
    /// <inheritdoc />
    public partial class ConversationID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConversationId",
                table: "MeetEvents",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "MeetEvents");
        }
    }
}
