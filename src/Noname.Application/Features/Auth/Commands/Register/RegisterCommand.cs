using MediatR;
using Noname.Application.Common.Models;

namespace Noname.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<Result<string>>;