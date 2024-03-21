using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        ConfigureQuestionTable(builder);
    }

    private void ConfigureQuestionTable(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id).HasConversion(
            question => question.Value,
            value => new QuestionId(value)
        );

        builder.HasMany(q => q.Problems)
            .WithOne()
            .HasForeignKey(q => q.QuestionId);
        
        builder.HasMany(q => q.Examinations)
            .WithOne()
            .HasForeignKey(q => q.QuestionId);
        
        builder.HasMany(q => q.Diagnostics)
            .WithMany();

        builder.HasMany(q => q.Treatments)
            .WithMany();
        
        builder.HasMany(q => q.Tags)
            .WithMany();

        builder.OwnsOne(q => q.Signalment);
        builder.Navigation(x => x.Signalment).IsRequired();

        builder.HasMany(q => q.Logs)
            .WithOne()
            .HasForeignKey(q => q.QuestionId);
    }
};