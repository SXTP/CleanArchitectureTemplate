using MediatR;
using Noname.Application.Common.Models;

namespace Noname.Application.Features.Auth.Queries.GetMe;

public record GetMeQuery : IRequest<Result<UserResponse>>;

public record UserResponse(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    string? ProfilePicture,
    IList<string> Roles);