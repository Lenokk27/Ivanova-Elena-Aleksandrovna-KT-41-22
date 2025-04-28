using IvanovaElenaAleksandrovnaKt_41_22.Filters.LoadFilters;
using IvanovaElenaAleksandrovnaKt_41_22.Interfaces.LoadInterfaces;
using IvanovaElenaAleksandrovnaKt_41_22.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace IvanovaElenaAleksandrovnaKt_41_22.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadsController : ControllerBase
    {
        private readonly ILogger<LoadsController> _logger;
        private readonly ILoadService _loadService;

        public LoadsController(ILogger<LoadsController> logger, ILoadService loadService)
        {
            _logger = logger;
            _loadService = loadService;
        }

        [HttpPost("get", Name = "GetLoads")]
        public async Task<IActionResult> GetLoadsAsync([FromBody] LoadFilter filter, CancellationToken cancellationToken)
        {
            var loads = await _loadService.GetLoadsAsync(filter, cancellationToken);
            return Ok(loads);
        }

        [HttpPost("add", Name = "AddLoad")]
        public async Task<IActionResult> AddLoadAsync([FromBody] Load load, CancellationToken cancellationToken)
        {
            await _loadService.AddLoadAsync(load, cancellationToken);
            return Ok("Нагрузка успешно добавлена.");
        }

        [HttpPut("update", Name = "UpdateLoad")]
        public async Task<IActionResult> UpdateLoadAsync([FromBody] Load load, CancellationToken cancellationToken)
        {
            await _loadService.UpdateLoadAsync(load, cancellationToken);
            return Ok("Нагрузка успешно обновлена.");
        }
    }
}