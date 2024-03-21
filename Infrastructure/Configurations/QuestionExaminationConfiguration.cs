using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class QuestionExaminationConfiguration : IEntityTypeConfiguration<QuestionExamination>
{
    public void Configure(EntityTypeBuilder<QuestionExamination> builder)
    {
        builder.HasKey(qe => qe.Id);

        builder.Property(qe => qe.Id).HasConversion(
            questionexamination => questionexamination.Value,
            value => new QuestionExaminationId(value)
        );

        builder.HasOne<Examination>()
            .WithMany()
            .HasForeignKey(qe => qe.ExaminationId);
        
        builder.Property(qe => qe.TextResult);
        builder.Property(qe => qe.ImgResult);

    }
}