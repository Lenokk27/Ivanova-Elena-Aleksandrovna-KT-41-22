using IvanovaElenaAleksandrovnaKt_41_22.Database;
using IvanovaElenaAleksandrovnaKt_41_22.Filters.LoadFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IvanovaElenaAleksandrovnaKt_41_22.Interfaces.LoadInterfaces
{
    public class LoadService : ILoadService
    {
        private readonly TeacherDbContext _dbContext;

        public LoadService(TeacherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Load[]> GetLoadsAsync(LoadFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<Load>()
                .Include(l => l.Teacher) // Включаем преподавателя
                .ThenInclude(t => t.Department) // Включаем кафедру преподавателя
                .Include(l => l.Discipline) // Включаем дисциплину
                .AsQueryable();

            // Применяем фильтры
            if (!string.IsNullOrEmpty(filter.TeacherName))
            {
                var teacherNameParts = filter.TeacherName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (teacherNameParts.Length == 2)
                {
                    var firstName = teacherNameParts[0];
                    var lastName = teacherNameParts[1];

                    query = query.Where(l => l.Teacher.FirstName == firstName && l.Teacher.LastName == lastName);
                }
            }

            if (!string.IsNullOrEmpty(filter.DepartmentName))
            {
                query = query.Where(l => l.Teacher.Department != null && l.Teacher.Department.Name == filter.DepartmentName);
            }

            if (!string.IsNullOrEmpty(filter.DisciplineName))
            {
                query = query.Where(l => l.Discipline.Name == filter.DisciplineName);
            }

            // Получаем результат
            var loads = await query.ToArrayAsync(cancellationToken);

            return loads;
        }

        public async Task AddLoadAsync(Load load, CancellationToken cancellationToken = default)
        {
            // Проверяем существование преподавателя и дисциплины
            var teacherExists = await _dbContext.Teachers.AnyAsync(t => t.Id == load.TeacherId, cancellationToken);
            var disciplineExists = await _dbContext.Disciplines.AnyAsync(d => d.Id == load.DisciplineId, cancellationToken);

            if (!teacherExists)
            {
                throw new InvalidOperationException("Преподаватель не найден.");
            }

            if (!disciplineExists)
            {
                throw new InvalidOperationException("Дисциплина не найдена.");
            }

            // Добавляем новую запись нагрузки
            await _dbContext.Loads.AddAsync(load, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateLoadAsync(Load load, CancellationToken cancellationToken = default)
        {
            var existingLoad = await _dbContext.Loads
                .FirstOrDefaultAsync(l => l.Id == load.Id, cancellationToken);

            if (existingLoad == null)
            {
                throw new InvalidOperationException("Нагрузка не найдена.");
            }

            // Обновляем свойства нагрузки
            existingLoad.Hours = load.Hours;
            existingLoad.TeacherId = load.TeacherId;
            existingLoad.DisciplineId = load.DisciplineId;

            // Сохраняем изменения
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}