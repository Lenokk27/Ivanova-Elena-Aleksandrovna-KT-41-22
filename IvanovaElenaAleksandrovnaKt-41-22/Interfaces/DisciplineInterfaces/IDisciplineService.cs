using IvanovaElenaAleksandrovnaKt_41_22.Filters.DisciplineFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Models;

namespace IvanovaElenaAleksandrovnaKt_41_22.Interfaces.DisciplineInterfaces
{
    public interface IDisciplineService
    {
        Task<Discipline[]> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken = default);
        Task AddDisciplineAsync(Discipline discipline, CancellationToken cancellationToken = default);
        Task UpdateDisciplineAsync(Discipline discipline, CancellationToken cancellationToken = default);
        Task DeleteDisciplineAsync(int disciplineId, CancellationToken cancellationToken = default);
    }
}