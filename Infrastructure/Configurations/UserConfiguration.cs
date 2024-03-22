using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasData(
            User.Create("Admin", "", "9999","admin1", "uC0vpYkNjo4q8C5")
        );
        builder.HasMany(u => u.RefreshTokens).WithOne();

    }
}
