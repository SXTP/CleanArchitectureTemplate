using MediatR;
using Noname.Application.Common.Interfaces;
using Noname.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noname.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (result, userId) = await _identityService.CheckPasswordAsync(request.Email, request.Password);

        if (!result.Succeeded)
        {
            return Result<LoginResponse>.Failure(result.Errors);
        }

        var roles = await _identityService.GetUserRolesAsync(userId);
        var token = _tokenService.GenerateJwtToken(userId, request.Email, roles);

        return Result<LoginResponse>.Success(new LoginResponse(token, request.Email, userId));
    }
}
