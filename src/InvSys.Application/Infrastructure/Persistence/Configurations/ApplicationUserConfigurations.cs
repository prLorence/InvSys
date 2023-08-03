using InvSys.Application.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvSys.Application.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // builder.ToTable("ApplicationUser");
        //
        // builder.HasKey(u => u.Id);

        // builder.HasMany(u => u.Claims)
        //        .WithOne()
        //        .HasForeignKey(uc => uc.)
        //        .IsRequired();
    }
}