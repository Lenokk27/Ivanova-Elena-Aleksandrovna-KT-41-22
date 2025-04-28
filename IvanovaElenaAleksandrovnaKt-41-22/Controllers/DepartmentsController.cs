using IvanovaElenaAleksandrovnaKt_41_22.Filters.DepartmentFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Interfaces.DepartmentInterfaces;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace IvanovaElenaAleksandrovnaKt_41_22.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly ILogger<DepartmentsController> _logger;
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(ILogger<DepartmentsController> logger, IDepartmentService departmentService)
        {
            _logger = logger;
            _departmentService = departmentService;
        }

        [HttpPost("get", Name = "GetDepartments")]
        public async Task<IActionResult> GetDepartmentsAsync([FromBody] DepartmentFilter filter, CancellationToken cancellationToken)
        {
            var departments = await _departmentService.GetDepartmentsAsync(filter, cancellationToken);
            return Ok(departments);
        }

        [HttpPost("add", Name = "AddDepartment")]
        public async Task<IActionResult> AddDepartmentAsync([FromBody] Department department, CancellationToken cancellationToken)
        {
            await _departmentService.AddDepartmentAsync(department, cancellationToken);
            return Ok("Кафедра успешно добавлена.");
        }

        [HttpPut("update", Name = "UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartmentAsync([FromBody] Department department, CancellationToken cancellationToken)
        {
            await _departmentService.UpdateDepartmentAsync(department, cancellationToken);
            return Ok("Кафедра успешно обновлена.");
        }

        [HttpDelete("delete/{departmentId}", Name = "DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartmentAsync(int departmentId, CancellationToken cancellationToken)
        {
            await _departmentService.DeleteDepartmentAsync(departmentId, cancellationToken);
            return Ok("Кафедра успешно удалена.");
        }
    }
}