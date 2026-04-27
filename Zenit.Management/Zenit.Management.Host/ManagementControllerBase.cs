using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Zenit.Management.Host
{
    [ApiController]
    public abstract class ManagementControllerBase : ControllerBase
    {
        protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();

        protected async Task<IActionResult> GetRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await Mediator.Send(request);
            
            return Ok(response);
        }

        protected async Task<IActionResult> CreateRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await Mediator.Send(request);
            return Created("", response);
        }

        protected async Task<IActionResult> UpdateRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await Mediator.Send(request);
            return Ok(response);
        }

        protected async Task<IActionResult> DeleteRequest<TRequest>(TRequest request)
            where TRequest : IRequest
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Mediator.Send(request);
            return Ok();
        }

        protected async Task<IActionResult> ActionRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await Mediator.Send(request);
            return Ok(response);
        }

        protected async Task<IActionResult> StreamRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Mediator.Send(request);
            return new EmptyResult();
        }
    }
}