using IvanovaElenaAleksandrovnaKt_41_22.Database;
using IvanovaElenaAleksandrovnaKt_41_22.Filters.DisciplineFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Interfaces.DisciplineInterfaces;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.EntityFrameworkCore;

namespace IvanovaElenaAleksandrovnaKt_41_22.xUnitTests
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
                var department = new Department { Name = "������� �����������" };
                var teacher = new Teacher
                {
                    FirstName = "����",
                    LastName = "������",
                    Department = department
                };

                var discipline1 = new Discipline { Name = "����������������" };
                var discipline2 = new Discipline { Name = "���� ������" };

                var load1 = new Load { Hours = 30, Teacher = teacher, Discipline = discipline1 };
                var load2 = new Load { Hours = 20, Teacher = teacher, Discipline = discipline2 };

                await context.Departments.AddAsync(department);
                await context.Teachers.AddAsync(teacher);
                await context.Disciplines.AddRangeAsync(discipline1, discipline2);
                await context.Loads.AddRangeAsync(load1, load2);
                await context.SaveChangesAsync();
            }

            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var disciplineService = new DisciplineService(context);
                var filter = new DepartmentDisciplineFilter { DepartmentName = "������� �����������" };

                // Act
                var result = await disciplineService.GetDisciplinesByDepartmentAsync(filter);

                // Assert
                Assert.Equal(2, result.Length);
                Assert.Contains(result, d => d.Name == "����������������");
                Assert.Contains(result, d => d.Name == "���� ������");
            }
        }

        [Fact]
        public async Task AddDisciplineAsync_AddsDisciplineSuccessfully()
        {
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var disciplineService = new DisciplineService(context);
                var discipline = new Discipline { Name = "����������" };

                await disciplineService.AddDisciplineAsync(discipline);

                var addedDiscipline = await context.Disciplines.FindAsync(discipline.Id);
                Assert.NotNull(addedDiscipline);
                Assert.Equal("����������", addedDiscipline.Name);
            }
        }

        [Fact]
        public async Task UpdateDisciplineAsync_UpdatesDisciplineSuccessfully()
        {
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var discipline = new Discipline { Name = "������" };
                await context.Disciplines.AddAsync(discipline);
                await context.SaveChangesAsync();

                var disciplineService = new DisciplineService(context);
                discipline.Name = "���������� ������";

                await disciplineService.UpdateDisciplineAsync(discipline);

                var updatedDiscipline = await context.Disciplines.FindAsync(discipline.Id);
                Assert.NotNull(updatedDiscipline);
                Assert.Equal("���������� ������", updatedDiscipline.Name);
            }
        }

        [Fact]
        public async Task DeleteDisciplineAsync_DeletesDisciplineSuccessfully()
        {
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var discipline = new Discipline { Name = "������� ����" };
                await context.Disciplines.AddAsync(discipline);
                await context.SaveChangesAsync();

                var disciplineService = new DisciplineService(context);
                await disciplineService.DeleteDisciplineAsync(discipline.Id);

                var deletedDiscipline = await context.Disciplines.FindAsync(discipline.Id);
                Assert.Null(deletedDiscipline);
            }
        }
    }
}