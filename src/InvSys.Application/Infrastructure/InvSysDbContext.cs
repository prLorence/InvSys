using System.Reflection;

using InvSys.Application.Common.Roles;
using InvSys.Application.Entities;
using InvSys.Application.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InvSys.Application.Infrastructure;

public class InvSysDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public InvSysDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvSysDbContext).Assembly);

        modelBuilder.Model.GetEntityTypes()
          .SelectMany(entity => entity.GetProperties())
          .Where(p => p.IsPrimaryKey())
          .ToList()
          .ForEach(p => p.ValueGenerated = ValueGenerated.Never);

        modelBuilder.SeedUsers();
    }


    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
}