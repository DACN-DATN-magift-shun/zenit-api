using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zenit.Statistics.Host
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StatisticsControllerBase : ControllerBase
    {
        public IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();

        public async Task<IActionResult> GetAllRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}