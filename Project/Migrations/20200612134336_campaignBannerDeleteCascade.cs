using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvertApi.Migrations
{
    public partial class campaignBannerDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banner_Campaign_IdCampaign",
                table: "Banner");

            migrationBuilder.AddForeignKey(
                name: "FK_Banner_Campaign_IdCampaign",
                table: "Banner",
                column: "IdCampaign",
                principalTable: "Campaign",
                principalColumn: "IdCampaign",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banner_Campaign_IdCampaign",
                table: "Banner");

            migrationBuilder.AddForeignKey(
                name: "FK_Banner_Campaign_IdCampaign",
                table: "Banner",
                column: "IdCampaign",
                principalTable: "Campaign",
                principalColumn: "IdCampaign",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
