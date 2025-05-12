using IvanovaElenaAleksandrovnaKt_41_22.Database;
using IvanovaElenaAleksandrovnaKt_41_22.Filters.LoadFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Interfaces.LoadInterfaces;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanovaElenaAleksandrovnaKt_41_22.Tests
{
    public class LoadServiceTests
    {
        private readonly DbContextOptions<TeacherDbContext> _dbContextOptions;

        public LoadServiceTests()
        {
            var dbName = $"TestDatabase_Load_{Guid.NewGuid()}";
            _dbContextOptions = new DbContextOptionsBuilder<TeacherDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using var context = new TeacherDbContext(_dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Fact]
        public async Task AddLoadAsync_AddsLoadSuccessfully()
        {
            // Arrange
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var loadService = new LoadService(context);

                var department = new Department { Name = "Кафедра информатики" };
                var teacher = new Teacher
                {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    Department = department
                };
                var discipline = new Discipline { Name = "Программирование" };

                await context.Departments.AddAsync(department);
                await context.Teachers.AddAsync(teacher);
                await context.Disciplines.AddAsync(discipline);
                await context.SaveChangesAsync();

                var load = new Load
                {
                    TeacherId = teacher.Id,
                    DisciplineId = discipline.Id,
                    Hours = 30
                };

                // Act
                await loadService.AddLoadAsync(load);

                // Assert
                var addedLoad = await context.Loads.FindAsync(load.Id);
                Assert.NotNull(addedLoad);
                Assert.Equal(30, addedLoad.Hours);
                Assert.Equal("Иван", addedLoad.Teacher.FirstName);
                Assert.Equal("Программирование", addedLoad.Discipline.Name);
            }
        }

        [Fact]
        public async Task UpdateLoadAsync_UpdatesLoadSuccessfully()
        {
            // Arrange
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var loadService = new LoadService(context);

                var department = new Department { Name = "Кафедра математики" };
                var teacher = new Teacher
                {
                    FirstName = "Петр",
                    LastName = "Петров",
                    Department = department
                };
                var discipline = new Discipline { Name = "Математический анализ" };

                await context.Departments.AddAsync(department);
                await context.Teachers.AddAsync(teacher);
                await context.Disciplines.AddAsync(discipline);
                await context.SaveChangesAsync();

                var load = new Load
                {
                    TeacherId = teacher.Id,
                    DisciplineId = discipline.Id,
                    Hours = 20
                };

                await context.Loads.AddAsync(load);
                await context.SaveChangesAsync();

                // Изменяем часы
                load.Hours = 40;

                // Act
                await loadService.UpdateLoadAsync(load);

                // Assert
                var updatedLoad = await context.Loads.FindAsync(load.Id);
                Assert.NotNull(updatedLoad);
                Assert.Equal(40, updatedLoad.Hours);
                Assert.Equal("Математический анализ", updatedLoad.Discipline.Name);
            }
        }

        [Fact]
        public async Task GetLoadsAsync_ReturnsFilteredLoads_ByTeacherName()
        {
            // Arrange
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var loadService = new LoadService(context);

                var department = new Department { Name = "Кафедра физики" };
                var teacher1 = new Teacher
                {
                    FirstName = "Александр",
                    LastName = "Смирнов",
                    Department = department
                };
                var teacher2 = new Teacher
                {
                    FirstName = "Алексей",
                    LastName = "Иванов",
                    Department = department
                };

                var discipline1 = new Discipline { Name = "Физика" };
                var discipline2 = new Discipline { Name = "Термодинамика" };

                await context.Departments.AddAsync(department);
                await context.Teachers.AddRangeAsync(teacher1, teacher2);
                await context.Disciplines.AddRangeAsync(discipline1, discipline2);
                await context.SaveChangesAsync();

                await context.Loads.AddRangeAsync(
                    new Load { TeacherId = teacher1.Id, DisciplineId = discipline1.Id, Hours = 30 },
                    new Load { TeacherId = teacher2.Id, DisciplineId = discipline2.Id, Hours = 40 }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var loadService = new LoadService(context);

                // Act
                var filter = new LoadFilter { TeacherName = "Александр Смирнов" };
                var result = await loadService.GetLoadsAsync(filter);

                // Assert
                Assert.Single(result);
                Assert.Equal("Физика", result.First().Discipline.Name);
            }
        }

        [Fact]
        public async Task UpdateLoadAsync_ThrowsKeyNotFoundException_WhenLoadDoesNotExist()
        {
            // Arrange
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var loadService = new LoadService(context);

                var department = new Department { Name = "Кафедра химии" };
                var teacher = new Teacher
                {
                    FirstName = "Василий",
                    LastName = "Пупкин",
                    Department = department
                };
                var discipline = new Discipline { Name = "Органическая химия" };

                await context.Departments.AddAsync(department);
                await context.Teachers.AddAsync(teacher);
                await context.Disciplines.AddAsync(discipline);
                await context.SaveChangesAsync();

                var invalidLoad = new Load
                {
                    Id = 999,
                    TeacherId = teacher.Id,
                    DisciplineId = discipline.Id,
                    Hours = 50
                };

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    loadService.UpdateLoadAsync(invalidLoad));
            }
        }
    }


}
