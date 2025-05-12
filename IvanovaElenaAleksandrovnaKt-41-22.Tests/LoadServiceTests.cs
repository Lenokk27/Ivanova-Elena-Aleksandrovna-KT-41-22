using IvanovaElenaAleksandrovnaKt_41_22.Database;
using IvanovaElenaAleksandrovnaKt_41_22.Filters.LoadFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Interfaces.LoadInterfaces;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using IvanovaElenaAleksandrovnaKt_41_22.Tests.Helpers;
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
                await TestDataSeeder.SeedAsync(context);
            }

            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var loadService = new LoadService(context);

                var teacher = await context.Teachers.FirstAsync();
                var discipline = await context.Disciplines.FirstAsync();

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
                Assert.Equal(teacher.Id, addedLoad.TeacherId);
                Assert.Equal(discipline.Id, addedLoad.DisciplineId);
            }
        }

        [Fact]
        public async Task UpdateLoadAsync_UpdatesLoadSuccessfully()
        {
            // Arrange
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                await TestDataSeeder.SeedAsync(context);
            }

            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var loadService = new LoadService(context);

                var existingLoad = await context.Loads.FirstAsync();
                existingLoad.Hours = 50;

                // Act
                await loadService.UpdateLoadAsync(existingLoad);

                // Assert
                var updatedLoad = await context.Loads.FindAsync(existingLoad.Id);
                Assert.NotNull(updatedLoad);
                Assert.Equal(50, updatedLoad.Hours);
            }
        }

        [Fact]
        public async Task GetLoadsAsync_ReturnsFilteredLoads_ByTeacherName()
        {
            // Arrange
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                await TestDataSeeder.SeedAsync(context);
            }

            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var loadService = new LoadService(context);

                // Act
                var filter = new LoadFilter { TeacherName = "Иван Иванов" };
                var result = await loadService.GetLoadsAsync(filter);

                // Assert
                Assert.Single(result);
                Assert.Equal("Программирование", result.First().Discipline.Name);
            }
        }

        [Fact]
        public async Task UpdateLoadAsync_ThrowsKeyNotFoundException_WhenLoadDoesNotExist()
        {
            // Arrange
            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                await TestDataSeeder.SeedAsync(context);
            }

            using (var context = new TeacherDbContext(_dbContextOptions))
            {
                var loadService = new LoadService(context);

                var invalidLoad = new Load
                {
                    Id = 999,
                    TeacherId = 1,
                    DisciplineId = 1,
                    Hours = 50
                };

                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    loadService.UpdateLoadAsync(invalidLoad));
            }
        }
    }

}
