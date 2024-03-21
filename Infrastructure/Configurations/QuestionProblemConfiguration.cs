using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class QuestionProblemConfiguration : IEntityTypeConfiguration<QuestionProblem>
{
    public void Configure(EntityTypeBuilder<QuestionProblem> builder)
    {
        builder.HasKey(qp => qp.Id);

        builder.Property(qp => qp.Id).HasConversion(
            questionproblem => questionproblem.Value,
            value => new QuestionProblemId(value)
        );

        builder.HasOne<Problem>()
            .WithMany()
            .HasForeignKey(qp => qp.ProblemId);
        
        builder.Property(qp => qp.Round);
    }
}
