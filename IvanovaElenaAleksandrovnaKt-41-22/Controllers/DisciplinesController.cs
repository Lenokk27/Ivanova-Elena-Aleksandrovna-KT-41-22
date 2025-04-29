using IvanovaElenaAleksandrovnaKt_41_22.Filters.DisciplineFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Interfaces.DisciplineInterfaces;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace IvanovaElenaAleksandrovnaKt_41_22.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisciplinesController : ControllerBase
    {
        private readonly ILogger<DisciplinesController> _logger;
        private readonly IDisciplineService _disciplineService;

        public DisciplinesController(ILogger<DisciplinesController> logger, IDisciplineService disciplineService)
        {
            _logger = logger;
            _disciplineService = disciplineService;
        }

        [HttpPost("get", Name = "GetDisciplines")]
        public async Task<IActionResult> GetDisciplinesAsync([FromBody] DisciplineFilter filter, CancellationToken cancellationToken)
        {
            var disciplines = await _disciplineService.GetDisciplinesAsync(filter, cancellationToken);
            return Ok(disciplines);
        }

        [HttpPost("add", Name = "AddDiscipline")]
        public async Task<IActionResult> AddDisciplineAsync([FromBody] Discipline discipline, CancellationToken cancellationToken)
        {
            await _disciplineService.AddDisciplineAsync(discipline, cancellationToken);
            return Ok("Дисциплина успешно добавлена.");
        }

        [HttpPut("update", Name = "UpdateDiscipline")]
        public async Task<IActionResult> UpdateDisciplineAsync([FromBody] Discipline discipline, CancellationToken cancellationToken)
        {
            await _disciplineService.UpdateDisciplineAsync(discipline, cancellationToken);
            return Ok("Дисциплина успешно обновлена.");
        }

        [HttpDelete("delete/{disciplineId}", Name = "DeleteDiscipline")]
        public async Task<IActionResult> DeleteDisciplineAsync(int disciplineId, CancellationToken cancellationToken)
        {
            await _disciplineService.DeleteDisciplineAsync(disciplineId, cancellationToken);
            return Ok("Дисциплина успешно удалена.");
        }
        [HttpGet("by-department")]
        public async Task<IActionResult> GetDisciplinesByDepartmentAsync(
    [FromQuery] string departmentName,
    CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(departmentName))
            {
                return BadRequest("Поле departmentName не может быть пустым.");
            }

            var filter = new DepartmentDisciplineFilter
            {
                DepartmentName = departmentName
            };

            var disciplines = await _disciplineService.GetDisciplinesByDepartmentAsync(filter, cancellationToken);

            return Ok(disciplines);
        }
    }
}