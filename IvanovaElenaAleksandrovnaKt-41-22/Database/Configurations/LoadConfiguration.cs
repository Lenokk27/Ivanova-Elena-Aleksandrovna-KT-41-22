using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvanovaElenaAleksandrovnaKt_41_22.Database.Configurations
{
    public class LoadConfiguration : IEntityTypeConfiguration<Load>
    {
        public void Configure(EntityTypeBuilder<Load> builder)
        {
            builder.ToTable("Loads");
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Hours).IsRequired();

            builder.HasOne(l => l.Teacher)
                .WithMany(t => t.Loads)
                .HasForeignKey(l => l.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Discipline)
                .WithMany(d => d.Loads)
                .HasForeignKey(l => l.DisciplineId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
