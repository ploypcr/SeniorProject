using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class QuestionLogConfiguration : IEntityTypeConfiguration<QuestionLog>
{
    public void Configure(EntityTypeBuilder<QuestionLog> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(q => q.Id).HasConversion(
            log => log.Value,
            value => new QuestionLogId(value)
        );

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(l => l.UserId);    

        builder.Property(l => l.DateTime);   
        
    }
}