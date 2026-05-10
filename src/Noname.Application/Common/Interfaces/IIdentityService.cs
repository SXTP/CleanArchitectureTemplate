using Noname.Application.Common.Models;
using Noname.Application.Features.Auth.Queries.GetMe;

namespace Noname.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string firstName, string lastName);

    Task<Result> DeleteUserAsync(string userId);

    Task<(Result Result, string UserId)> CheckPasswordAsync(string email, string password);

    Task<IList<string>> GetUserRolesAsync(string userId);

    Task<Result<UserResponse>> GetUserProfileAsync(string userId);
}