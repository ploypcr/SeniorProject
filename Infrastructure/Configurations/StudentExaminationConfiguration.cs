using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class StudentExaminationConfiguration : IEntityTypeConfiguration<StudentExamination>
{
    public void Configure(EntityTypeBuilder<StudentExamination> builder)
    {
        builder.HasKey(se => se.Id);

        builder.Property(se => se.Id).HasConversion(
            Id => Id.Value,
            value => new StudentExaminationId(value)
        );

        builder.HasOne<Examination>()
            .WithMany()
            .HasForeignKey(se => se.ExaminationId);
            
    }
}