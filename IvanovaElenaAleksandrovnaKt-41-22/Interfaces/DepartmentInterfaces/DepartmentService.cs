using IvanovaElenaAleksandrovnaKt_41_22.Database;
using IvanovaElenaAleksandrovnaKt_41_22.Filters.DepartmentFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IvanovaElenaAleksandrovnaKt_41_22.Interfaces.DepartmentInterfaces
{
    public class DepartmentService : IDepartmentService
    {
        private readonly TeacherDbContext _dbContext;

        public DepartmentService(TeacherDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Department[]> GetDepartmentsAsync(DepartmentFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<Department>()
                .Include(d => d.Head) // Включаем заведующего кафедрой
                .Include(d => d.Teachers) // Включаем список преподавателей
                .AsQueryable();

            // Применяем фильтры
            if (filter.FoundationDate.HasValue)
            {
                query = query.Where(d => d.FoundationDate == filter.FoundationDate.Value);
            }

            if (filter.MinTeacherCount.HasValue)
            {
                query = query.Where(d => d.Teachers.Count >= filter.MinTeacherCount.Value);
            }

            var departments = await query.ToArrayAsync(cancellationToken);

            return departments;
        }

        public async Task AddDepartmentAsync(Department department, CancellationToken cancellationToken = default)
        {
            await _dbContext.Departments.AddAsync(department, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateDepartmentAsync(Department department, CancellationToken cancellationToken = default)
        {
            var existingDepartment = await _dbContext.Departments
                .Include(d => d.Teachers)
                .FirstOrDefaultAsync(d => d.Id == department.Id, cancellationToken);

            if (existingDepartment == null)
            {
                throw new InvalidOperationException("Кафедра не найдена.");
            }

            // Обновляем свойства кафедры
            existingDepartment.Name = department.Name;
            existingDepartment.FoundationDate = department.FoundationDate;
            existingDepartment.HeadId = department.HeadId;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            // Находим кафедру с включенными преподавателями
            var department = await _dbContext.Departments
                .Include(d => d.Teachers) // Включаем преподавателей
                .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);

            // Если кафедра не найдена, возвращаем false
            if (department == null)
            {
                return await Task.FromResult(false); // Используем await для асинхронного возврата false
            }

            // Удаляем связь с заведующим кафедрой
            department.HeadId = null;
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Удаляем всех преподавателей, связанных с кафедрой
            _dbContext.Teachers.RemoveRange(department.Teachers);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Удаляем саму кафедру
            _dbContext.Departments.Remove(department);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true; // Здесь можно оставить просто true, так как это уже внутри асинхронного метода
        }
    }
}