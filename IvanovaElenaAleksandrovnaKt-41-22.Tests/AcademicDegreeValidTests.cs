using IvanovaElenaAleksandrovnaKt_41_22.Database;
using IvanovaElenaAleksandrovnaKt_41_22.Tests.Helpers.Validators;
using IvanovaElenaAleksandrovnaKt_41_22.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanovaElenaAleksandrovnaKt_41_22.Tests
{
    public class AcademicDegreeValidTests
    {
        [Fact]
        public async Task AllSeededAcademicDegrees_ShouldHaveValidNames()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TeacherDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_AcademicDegreeValidation")
                .Options;

            using var context = new TeacherDbContext(options);
            await TestDataSeeder.SeedAsync(context);

            // Act
            var degrees = await context.AcademicDegrees.ToListAsync();
            var allValid = degrees.All(degree => AcademicDegreeValidator.IsValidName(degree.Title));

            // Assert
            Assert.True(allValid, "Все названия учёных степеней должны соответствовать формату 'Заглавное слово строчное слово'");
        }
    }

}
