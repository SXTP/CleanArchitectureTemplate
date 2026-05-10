using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Noname.Application.Common.Interfaces;
using Noname.Application.Common.Models;
using Noname.Application.Features.Auth.Queries.GetMe;
using Noname.Domain.Entities;
using Noname.Domain.Enums;

namespace Noname.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    #region ctor
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly IApplicationDbContext _context;

    public IdentityService(
        UserManager<AppUser> userManager,
        IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IApplicationDbContext context)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _context = context;
    }
    #endregion

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user?.UserName;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, string firstName, string lastName)
    {
        var user = new AppUser
        {
            UserName = userName,
            Email = userName,
            Member = new Member
            {
                FirstName = firstName,
                LastName = lastName
            }
        };

        user.Member.CreatedById = user.Id;
        user.Member.Status = EntityStatus.Active;
        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.Users.Include(u => u.Member).FirstOrDefaultAsync(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(AppUser user)
    {
        user.LockoutEnd = DateTimeOffset.MaxValue;

        if (user.Member != null)
            user.Member.IsDeleted = true;

        var result = await _userManager.UpdateAsync(user);

        return result.ToApplicationResult();
    }

    public async Task<(Result Result, string UserId)> CheckPasswordAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return (Result.Failure(new[] { "Kullanıcı bulunamadı." }), string.Empty);
        }

        var isValid = await _userManager.CheckPasswordAsync(user, password);

        return isValid
            ? (Result.Success(), user.Id)
            : (Result.Failure(new[] { "Geçersiz şifre." }), string.Empty);
    }

    public async Task<IList<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return new List<string>();
        }

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<Result<UserResponse>> GetUserProfileAsync(string userId)
    {
        var userDetails = await (from u in _userManager.Users
                                 join member in _context.Members on u.Id equals member.IdentityId
                                 where u.Id == userId
                                 select new
                                 {
                                     u.Id,
                                     u.Email,
                                     member.FirstName,
                                     member.LastName,
                                     member.ProfilePicture
                                 }).FirstOrDefaultAsync();

        if (userDetails == null)
            return Result<UserResponse>.Failure(new[] { "Kullanıcı profili bulunamadı." });

        var user = await _userManager.FindByIdAsync(userId);
        var roles = user != null ? await _userManager.GetRolesAsync(user) : new List<string>();

        var response = new UserResponse(
            userDetails.Id,
            userDetails.Email!,
            userDetails.FirstName,
            userDetails.LastName,
            $"{userDetails.FirstName} {userDetails.LastName}",
            userDetails.ProfilePicture,
            roles
        );

        return Result<UserResponse>.Success(response);
    }
}
