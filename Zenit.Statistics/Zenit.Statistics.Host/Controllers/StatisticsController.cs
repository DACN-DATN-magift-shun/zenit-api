using System.Text.Json;

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
        [HttpGet("stream")]
        public async Task<IActionResult> GetAll([FromQuery] StatisticsGetAllRequest request)
        {
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.ContentType = "text/event-stream";

            while (!HttpContext.RequestAborted.IsCancellationRequested)
            {
                var response = await GetAllRequest<StatisticsGetAllRequest, StatisticsGetAllResponse>(request);

                await Response.WriteAsync($"data: {JsonSerializer.Serialize(response)}\n\n");   // ghi event vào buffer
                await Response.Body.FlushAsync();                    // ép gửi ngay xuống client

                await Task.Delay(2000); // chờ 2 giây trước khi gửi event tiếp theo
            }

            return Ok();
        }
    }
}