using IvanovaElenaAleksandrovnaKt_41_22.Database;
using IvanovaElenaAleksandrovnaKt_41_22.Filters.TeacherFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IvanovaElenaAleksandrovnaKt_41_22.Interfaces.TeachersInterfaces
{
    public class TeacherService : ITeacherService
    {
        private readonly TeacherDbContext _dbContext;

        public TeacherService(TeacherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Teacher[]> GetTeachersAsync(TeacherFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<Teacher>()
                .Include(t => t.Department) // Включаем кафедру
                .Include(t => t.AcademicDegree) // Включаем академическую степень
                .Include(t => t.Position) // Включаем должность
                .AsQueryable();

            // Применяем фильтры
            if (!string.IsNullOrEmpty(filter.DepartmentName))
            {
                query = query.Where(t => t.Department != null && t.Department.Name == filter.DepartmentName);
            }

            if (!string.IsNullOrEmpty(filter.AcademicDegreeTitle))
            {
                query = query.Where(t => t.AcademicDegree != null && t.AcademicDegree.Title == filter.AcademicDegreeTitle);
            }

            if (!string.IsNullOrEmpty(filter.PositionTitle))
            {
                query = query.Where(t => t.Position != null && t.Position.Title == filter.PositionTitle);
            }

            // Получаем результат
            var teachers = await query.ToArrayAsync(cancellationToken);

            return teachers;
        }

        public async Task AddTeacherAsync(Teacher teacher, CancellationToken cancellationToken = default)
        {
            await _dbContext.Teachers.AddAsync(teacher, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTeacherAsync(Teacher teacher, CancellationToken cancellationToken = default)
        {
            var existingTeacher = await _dbContext.Teachers
                .Include(t => t.Loads) // Включаем нагрузку для обновления связей
                .FirstOrDefaultAsync(t => t.Id == teacher.Id, cancellationToken);

            if (existingTeacher == null)
            {
                throw new InvalidOperationException("Преподаватель не найден.");
            }

            // Обновляем свойства преподавателя
            existingTeacher.FirstName = teacher.FirstName;
            existingTeacher.LastName = teacher.LastName;
            existingTeacher.AcademicDegreeId = teacher.AcademicDegreeId;
            existingTeacher.PositionId = teacher.PositionId;
            existingTeacher.DepartmentId = teacher.DepartmentId;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteTeacherAsync(int teacherId, CancellationToken cancellationToken = default)
        {
            var teacher = await _dbContext.Teachers
                .Include(t => t.Loads) // Включаем нагрузку для каскадного удаления
                .FirstOrDefaultAsync(t => t.Id == teacherId, cancellationToken);

            if (teacher == null)
            {
                throw new InvalidOperationException("Преподаватель не найден.");
            }

            // Удаляем все связанные нагрузки
            _dbContext.Loads.RemoveRange(teacher.Loads);

            // Удаляем самого преподавателя
            _dbContext.Teachers.Remove(teacher);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}