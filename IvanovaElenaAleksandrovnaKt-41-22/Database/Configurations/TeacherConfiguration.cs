using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvanovaElenaAleksandrovnaKt_41_22.Database.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("Teachers");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(t => t.LastName).HasMaxLength(50).IsRequired();

            builder.HasOne(t => t.AcademicDegree)
                .WithMany(d => d.Teachers)
                .HasForeignKey(t => t.AcademicDegreeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(t => t.Position)
                .WithMany(p => p.Teachers)
                .HasForeignKey(t => t.PositionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(t => t.Department)
                .WithMany(d => d.Teachers)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
