using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zenit.Statistics.Contract.Requests;

namespace Zenit.Statistics.Host.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StatisticsController : StatisticsControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] StatisticsGetAllRequest request)
        {
            return await GetRequest<StatisticsGetAllRequest, StatisticsGetAllResponse>(request);
        }
    }
}