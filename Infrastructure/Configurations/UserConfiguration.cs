using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BC = BCrypt.Net.BCrypt;
namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(t => t.Id);

        var salt = BC.GenerateSalt(10);
        var hashedPassword = BC.HashPassword("uC0vpYkNjo4q8C5",salt);
        builder.HasData(
            User.Create("Admin", "", "9999","admin1", hashedPassword, null, true, "Admin")
        );
        
        builder.HasMany(u => u.RefreshTokens).WithOne().HasForeignKey(u => u.UserId);

    }
}
