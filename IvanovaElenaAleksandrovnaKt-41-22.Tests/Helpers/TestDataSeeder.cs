using IvanovaElenaAleksandrovnaKt_41_22.Database;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvanovaElenaAleksandrovnaKt_41_22.Tests.Helpers
{
    public static class TestDataSeeder
    {
        public static async Task SeedAsync(TeacherDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Кафедры
            var departments = new List<Department>
            {
                new Department { Name = "Кафедра информатики" },
                new Department { Name = "Кафедра математики" }
            };

            await context.Departments.AddRangeAsync(departments);
            await context.SaveChangesAsync();

            // Ученые степени
            var academicDegrees = new List<AcademicDegree>
            {
                new AcademicDegree { Title = "Кандидат наук" },
                new AcademicDegree { Title = "Доктор наук" }
            };

            await context.AcademicDegrees.AddRangeAsync(academicDegrees);

            // Должности
            var positions = new List<Position>
            {
                new Position { Title = "Доцент" },
                new Position { Title = "Профессор" }
            };

            await context.Positions.AddRangeAsync(positions);
            await context.SaveChangesAsync();

            // Преподаватели
            var teachers = new List<Teacher>
            {
                new Teacher
                {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    DepartmentId = departments[0].Id,
                    AcademicDegreeId = academicDegrees[0].Id,
                    PositionId = positions[0].Id
                },
                new Teacher
                {
                    FirstName = "Петр",
                    LastName = "Петров",
                    DepartmentId = departments[1].Id,
                    AcademicDegreeId = academicDegrees[0].Id,
                    PositionId = positions[0].Id
                },
                new Teacher
                {
                    FirstName = "Сергей",
                    LastName = "Сергеев",
                    DepartmentId = departments[1].Id,
                    AcademicDegreeId = academicDegrees[1].Id,
                    PositionId = positions[1].Id
                }
            };

            await context.Teachers.AddRangeAsync(teachers);

            // Назначаем заведующих кафедрами
            departments[0].HeadId = teachers[0].Id;
            departments[1].HeadId = teachers[1].Id;
            context.Departments.UpdateRange(departments);
            await context.SaveChangesAsync();

            // Дисциплины
            var disciplines = new List<Discipline>
            {
                new Discipline { Name = "Программирование" },
                new Discipline { Name = "Математический анализ" }
            };

            await context.Disciplines.AddRangeAsync(disciplines);

            // Нагрузка
            var loads = new List<Load>
            {
                new Load { Hours = 40, TeacherId = teachers[0].Id, DisciplineId = disciplines[0].Id },
                new Load { Hours = 35, TeacherId = teachers[2].Id, DisciplineId = disciplines[0].Id },
                new Load { Hours = 30, TeacherId = teachers[2].Id, DisciplineId = disciplines[1].Id }
            };

            await context.Loads.AddRangeAsync(loads);
            await context.SaveChangesAsync();
        }
    }

}
