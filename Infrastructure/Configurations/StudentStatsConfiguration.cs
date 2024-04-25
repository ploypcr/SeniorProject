using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class StudentSelectionConfiguraation : IEntityTypeConfiguration<StudentStats>
{
    public void Configure(EntityTypeBuilder<StudentStats> builder)
    {
        ConfigureQuestionTable(builder);
    }


    private void ConfigureQuestionTable(EntityTypeBuilder<StudentStats> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasConversion(
            id => id.Value,
            value => new StudentStatsId(value)
        );

        builder.Property(s => s.QuesVersion).HasDefaultValue(1.1);
        
        builder.HasOne<Question>()
            .WithMany()
            .HasForeignKey(s => s.QuestionId);

        builder.HasOne<User>()
            .WithMany();

        builder.HasMany(s => s.Examinations)
            .WithOne()
            .HasForeignKey(q => q.StudentStatsId);
        
        builder.HasMany(s => s.Problems)
            .WithOne()
            .HasForeignKey(q => q.StudentStatsId);
        
        builder.HasMany(s => s.Diagnostics)
            .WithMany();

        builder.HasMany(s => s.Treatments)
            .WithMany();

        builder.Property(s => s.ExtraAns).HasDefaultValue("");
        
    }
};