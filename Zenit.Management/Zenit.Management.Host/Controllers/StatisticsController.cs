using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Management.Contract.Requests.StatisticsRequests;

namespace Zenit.Management.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StatisticsController : ManagementControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] StatisticsGetAllRequest request)
        {
            return await GetRequest<StatisticsGetAllRequest, StatisticsGetAllResponse>(request);
        }

        [HttpPost("Reports")]
        public async Task<IActionResult> GenerateReport([FromQuery] ReportCreateRequest request)
        {
            return await CreateRequest<ReportCreateRequest, ReportCreateResponse>(request);
        }
    }
}