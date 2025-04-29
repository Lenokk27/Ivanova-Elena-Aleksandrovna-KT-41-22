using IvanovaElenaAleksandrovnaKt_41_22.Database;
using IvanovaElenaAleksandrovnaKt_41_22.Filters.DisciplineFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IvanovaElenaAleksandrovnaKt_41_22.Interfaces.DisciplineInterfaces
{
    public class DisciplineService : IDisciplineService
    {
        private readonly TeacherDbContext _dbContext;

        public DisciplineService(TeacherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Discipline[]> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<Discipline>()
                .Include(d => d.Loads) // Включаем нагрузку
                .ThenInclude(l => l.Teacher) // Включаем преподавателя
                .AsQueryable();

            // Применяем фильтры
            if (!string.IsNullOrEmpty(filter.TeacherName))
            {
                var teacherNameParts = filter.TeacherName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (teacherNameParts.Length == 2)
                {
                    var firstName = teacherNameParts[0];
                    var lastName = teacherNameParts[1];

                    query = query.Where(d => d.Loads.Any(l =>
                        l.Teacher.FirstName == firstName &&
                        l.Teacher.LastName == lastName));
                }
            }

            if (filter.MinHours.HasValue)
            {
                query = query.Where(d => d.Loads.Any(l => l.Hours >= filter.MinHours.Value));
            }

            if (filter.MaxHours.HasValue)
            {
                query = query.Where(d => d.Loads.Any(l => l.Hours <= filter.MaxHours.Value));
            }

            // Получаем результат
            var disciplines = await query.ToArrayAsync(cancellationToken);

            return disciplines;
        }

        public async Task AddDisciplineAsync(Discipline discipline, CancellationToken cancellationToken = default)
        {
            await _dbContext.Disciplines.AddAsync(discipline, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateDisciplineAsync(Discipline discipline, CancellationToken cancellationToken = default)
        {
            var existingDiscipline = await _dbContext.Disciplines
                .Include(d => d.Loads) // Включаем нагрузку для обновления связей
                .FirstOrDefaultAsync(d => d.Id == discipline.Id, cancellationToken);

            if (existingDiscipline == null)
            {
                throw new InvalidOperationException("Дисциплина не найдена.");
            }

            // Обновляем свойства дисциплины
            existingDiscipline.Name = discipline.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteDisciplineAsync(int disciplineId, CancellationToken cancellationToken = default)
        {
            var discipline = await _dbContext.Disciplines
                .Include(d => d.Loads) // Включаем нагрузку для каскадного удаления
                .FirstOrDefaultAsync(d => d.Id == disciplineId, cancellationToken);

            if (discipline == null)
            {
                throw new InvalidOperationException("Дисциплина не найдена.");
            }

            // Удаляем все связанные нагрузки
            _dbContext.Loads.RemoveRange(discipline.Loads);

            // Удаляем саму дисциплину
            _dbContext.Disciplines.Remove(discipline);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<Discipline[]> GetDisciplinesByDepartmentAsync(DepartmentDisciplineFilter filter, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(filter.DepartmentName))
                return Array.Empty<Discipline>();

            var disciplines = await (
                from department in _dbContext.Departments
                where department.Name == filter.DepartmentName
                join teacher in _dbContext.Teachers on department.Id equals teacher.DepartmentId
                join load in _dbContext.Loads on teacher.Id equals load.TeacherId
                join discipline in _dbContext.Disciplines on load.DisciplineId equals discipline.Id
                select discipline
            ).Distinct()
             .ToArrayAsync(cancellationToken);

            return disciplines;
        }
    }
}