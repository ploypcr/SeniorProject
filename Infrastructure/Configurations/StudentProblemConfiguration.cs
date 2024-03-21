using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class StudentProblemConfiguration : IEntityTypeConfiguration<StudentProblem>
{
    public void Configure(EntityTypeBuilder<StudentProblem> builder)
    {
        builder.HasKey(se => se.Id);

        builder.Property(se => se.Id).HasConversion(
            Id => Id.Value,
            value => new StudentProblemId(value)
        );

        builder.HasOne<Problem>()
            .WithMany()
            .HasForeignKey(e => e.ProblemId);

        builder.Property(se => se.Round);
    }
}