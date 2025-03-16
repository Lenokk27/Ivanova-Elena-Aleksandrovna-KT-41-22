using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvanovaElenaAleksandrovnaKt_41_22.Database.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name).HasMaxLength(100).IsRequired();

            builder.HasOne(d => d.Head)
            .WithOne(t => t.ManagedDepartment)
            .HasForeignKey<Department>(d => d.HeadId)
            .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
