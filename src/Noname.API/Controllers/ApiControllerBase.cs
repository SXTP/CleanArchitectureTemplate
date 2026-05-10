using MediatR;
using Microsoft.AspNetCore.Mvc;
using Noname.Application.Common.Models;

namespace Noname.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null) return NotFound();

        if (result.Succeeded)
            return Ok(result);

        return BadRequest(result);
    }

    protected ActionResult HandleResult(Result result)
    {
        if (result == null) return NotFound();

        if (result.Succeeded)
            return Ok(result);

        return BadRequest(result);
    }
}