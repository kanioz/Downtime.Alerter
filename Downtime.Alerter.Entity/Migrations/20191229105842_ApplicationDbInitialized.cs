using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Downtime.Alerter.Entity.Migrations
{
    public partial class ApplicationDbInitialized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetApplications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ApplicationName = table.Column<string>(nullable: true),
                    ApplicationUrl = table.Column<string>(nullable: true),
                    MonitoringInterval = table.Column<int>(nullable: false),
                    LastMonitoringTime = table.Column<DateTime>(nullable: false),
                    NextMonitoringTime = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetApplications_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MonitoringHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonitoringTime = table.Column<DateTime>(nullable: false),
                    IsUp = table.Column<bool>(nullable: false),
                    StatusCode = table.Column<string>(nullable: true),
                    TargetApplicationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitoringHistories_TargetApplications_TargetApplicationId",
                        column: x => x.TargetApplicationId,
                        principalTable: "TargetApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TargetApplicationNotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetApplicationId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetApplicationNotificationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetApplicationNotificationTypes_TargetApplications_TargetApplicationId",
                        column: x => x.TargetApplicationId,
                        principalTable: "TargetApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    NotificationType = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    MonitoringHistoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_MonitoringHistories_MonitoringHistoryId",
                        column: x => x.MonitoringHistoryId,
                        principalTable: "MonitoringHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringHistories_TargetApplicationId",
                table: "MonitoringHistories",
                column: "TargetApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_MonitoringHistoryId",
                table: "Notifications",
                column: "MonitoringHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetApplicationNotificationTypes_TargetApplicationId",
                table: "TargetApplicationNotificationTypes",
                column: "TargetApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetApplications_OwnerId",
                table: "TargetApplications",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "TargetApplicationNotificationTypes");

            migrationBuilder.DropTable(
                name: "MonitoringHistories");

            migrationBuilder.DropTable(
                name: "TargetApplications");
        }
    }
}
