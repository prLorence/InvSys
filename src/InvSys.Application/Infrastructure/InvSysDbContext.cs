using System.Reflection;

using InvSys.Application.Common.Roles;
using InvSys.Application.Entities;

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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvSysDbContext).Assembly);

        modelBuilder.Model.GetEntityTypes()
          .SelectMany(entity => entity.GetProperties())
          .Where(p => p.IsPrimaryKey())
          .ToList()
          .ForEach(p => p.ValueGenerated = ValueGenerated.Never);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
}