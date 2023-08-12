using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InvSys.Application.Migrations
{
    /// <inheritdoc />
    public partial class SeededUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"), null, "Admin", "ADMIN" },
                    { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), null, "InventoryManager", "INVENTORYMANAGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"), 0, "5919bc29-3dc4-4c0e-a89b-706a818938eb", "admin@invsys.com", true, false, null, "ADMIN@INVSYS.COM", "INVSYS-ADMIN", "AQAAAAIAAYagAAAAEJ9VbrFq0kmb81By2wZ60mPW1c+sYeGtiw3KnyWRbtHcrLSAvFGVM9Ag2bFFjq56jA==", null, false, null, false, "invsys-admin" },
                    { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), 0, "a61d2f3b-8677-45c5-918d-0539d8861178", "inv-manager@invsys.com", true, false, null, "INV-MANAGER@INVSYS.COM", "INVSYS-INV-MANAGER", "AQAAAAIAAYagAAAAEIlV3ZSEkGxWJ6by8SKuBTQSDp65Lnm5xH7enIBP4JhvrNTO1jNehUdWG21KO4oKVg==", null, false, null, false, "invsys-inv-manager" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"), new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210") },
                    { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"), new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"));
        }
    }
}
