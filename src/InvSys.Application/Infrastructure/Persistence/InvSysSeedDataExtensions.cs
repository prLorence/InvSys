using InvSys.Application.Common.Roles;
using InvSys.Application.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Application.Infrastructure.Persistence;

public static class InvSysSeedDataExtensions
{
    public static void SeedUsers(this ModelBuilder modelBuilder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        var ADMIN_ID = Guid.Parse("2c5e174e-3b0e-446f-86af-483d56fd7210");
        var IM_ID = Guid.Parse("a18be9c0-aa65-4af8-bd17-00bd9344e575");

        var ADMIN_ROLE_ID = ADMIN_ID;
        var IM_ROLE_ID = IM_ID;

        var admin = new ApplicationUser
        {
            Id = ADMIN_ID,
            UserName = "invsys-admin",
            Email = "admin@invsys.com",
            NormalizedEmail = "admin@invsys.com".ToUpper(),
            NormalizedUserName = "invsys-admin".ToUpper(),
            TwoFactorEnabled = false,
            EmailConfirmed = true,
        };

        var inventoryManager = new ApplicationUser
        {
            Id = IM_ID,
            UserName = "invsys-inv-manager",
            Email = "inv-manager@invsys.com",
            NormalizedEmail = "inv-manager@invsys.com".ToUpper(),
            NormalizedUserName = "invsys-inv-manager".ToUpper(),
            TwoFactorEnabled = false,
            EmailConfirmed = true,
        };

        admin.PasswordHash = hasher.HashPassword(admin, "inv-sys.admin");
        inventoryManager.PasswordHash = hasher.HashPassword(inventoryManager, "inv-sys.manager");

        modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = ADMIN_ROLE_ID, Name = "Admin", NormalizedName = "ADMIN" },
                new ApplicationRole { Id = IM_ROLE_ID, Name = "InventoryManager", NormalizedName = "INVENTORYMANAGER" }
                );

        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { RoleId = ADMIN_ROLE_ID, UserId = ADMIN_ID },
                new IdentityUserRole<Guid> { RoleId = IM_ROLE_ID, UserId = IM_ID }
                );

        modelBuilder.Entity<ApplicationUser>().HasData(admin, inventoryManager);
    }
}