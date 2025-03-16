using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvanovaElenaAleksandrovnaKt_41_22.Database.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Title).HasMaxLength(100).IsRequired();
        }

    }
}
