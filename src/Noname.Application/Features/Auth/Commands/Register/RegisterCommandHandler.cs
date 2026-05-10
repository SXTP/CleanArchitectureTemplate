using MediatR;
using Noname.Application.Common.Interfaces; // IdentityService interface'in
using Noname.Application.Common.Models;

namespace Noname.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string>>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Infrastructure katmanındaki servisi çağırıyoruz
        var (result, userId) = await _identityService.CreateUserAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName);

        if (!result.Succeeded)
        {
            return Result<string>.Failure(result.Errors);
        }

        return Result<string>.Success(userId);
    }
}