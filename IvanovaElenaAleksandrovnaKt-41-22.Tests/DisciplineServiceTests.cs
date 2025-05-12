using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using IvanovaElenaAleksandrovnaKt_41_22.Database;
using IvanovaElenaAleksandrovnaKt_41_22.Filters.DisciplineFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Interfaces.DisciplineInterfaces;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using IvanovaElenaAleksandrovnaKt_41_22.Tests.Helpers;


namespace IvanovaElenaAleksandrovnaKt_41_22.Tests
{
    public class DisciplineServiceTests
    {
        private readonly DbContextOptions<TeacherDbContext> _dbContextOptions;

        public DisciplineServiceTests()
        {
            var dbName = $"TestDatabase_Discipline_{Guid.NewGuid()}";
            _dbContextOptions = new DbContextOptionsBuilder<TeacherDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using var context = new TeacherDbContext(_dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetDisciplinesByDepartmentAsync_ReturnsDisciplinesForGivenDepartment()
        {
            // Arrange
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                await TestDataSeeder.SeedAsync(context);
            }

            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var disciplineService = new DisciplineService(context);
                var filter = new DepartmentDisciplineFilter { DepartmentName = "Кафедра информатики" };

                // Act
                var result = await disciplineService.GetDisciplinesByDepartmentAsync(filter);

                // Assert
                Assert.Single(result); // или Assert.Equal(1, result.Length);
                Assert.Contains(result, d => d.Name == "Программирование");
            }
        }

        [Fact]
        public async Task AddDisciplineAsync_AddsDisciplineSuccessfully()
        {
            // Arrange
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                await TestDataSeeder.SeedAsync(context);
            }

            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var disciplineService = new DisciplineService(context);
                var discipline = new Discipline { Name = "Математика" };

                // Act
                await disciplineService.AddDisciplineAsync(discipline);

                var addedDiscipline = await context.Disciplines.FindAsync(discipline.Id);

                // Assert
                Assert.NotNull(addedDiscipline);
                Assert.Equal("Математика", addedDiscipline.Name);
            }
        }

        [Fact]
        public async Task UpdateDisciplineAsync_UpdatesDisciplineSuccessfully()
        {
            // Arrange
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                await TestDataSeeder.SeedAsync(context);
            }

            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var disciplineService = new DisciplineService(context);
                var existingDiscipline = await context.Disciplines.FirstAsync();

                existingDiscipline.Name = "Обновлённая физика";

                // Act
                await disciplineService.UpdateDisciplineAsync(existingDiscipline);

                var updatedDiscipline = await context.Disciplines.FindAsync(existingDiscipline.Id);

                // Assert
                Assert.NotNull(updatedDiscipline);
                Assert.Equal("Обновлённая физика", updatedDiscipline.Name);
            }
        }

        [Fact]
        public async Task DeleteDisciplineAsync_DeletesDisciplineSuccessfully()
        {
            // Arrange
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                await TestDataSeeder.SeedAsync(context);
            }

            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var disciplineService = new DisciplineService(context);
                var disciplineToDelete = await context.Disciplines.FirstAsync();

                // Act
                await disciplineService.DeleteDisciplineAsync(disciplineToDelete.Id);

                var deletedDiscipline = await context.Disciplines.FindAsync(disciplineToDelete.Id);

                // Assert
                Assert.Null(deletedDiscipline);
            }
        }
    }
}