using IvanovaElenaAleksandrovnaKt_41_22.Models;
using IvanovaElenaAleksandrovnaKt_41_22.Database.Configurations;

using Microsoft.EntityFrameworkCore;

namespace IvanovaElenaAleksandrovnaKt_41_22.Database
{
    public class TeacherDbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<AcademicDegree> AcademicDegrees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Load> Loads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new AcademicDegreeConfiguration());
            modelBuilder.ApplyConfiguration(new PositionConfiguration());
            modelBuilder.ApplyConfiguration(new DisciplineConfiguration());
            modelBuilder.ApplyConfiguration(new LoadConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public TeacherDbContext(DbContextOptions<TeacherDbContext> options) : base(options)
        {
        }
    }
}
