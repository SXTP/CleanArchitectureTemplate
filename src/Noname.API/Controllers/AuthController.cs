using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noname.Application.Features.Auth.Commands.Login;
using Noname.Application.Features.Auth.Commands.Register;
using Noname.Application.Features.Auth.Queries.GetMe;

namespace Noname.API.Controllers;

public class AuthController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserResponse>> GetMe()
    {
        var result = await Mediator.Send(new GetMeQuery());

        return HandleResult(result);
    }
}