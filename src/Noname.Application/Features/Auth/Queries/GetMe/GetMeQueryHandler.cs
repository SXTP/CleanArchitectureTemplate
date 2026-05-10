using MediatR;
using Noname.Application.Common.Interfaces;
using Noname.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noname.Application.Features.Auth.Queries.GetMe;

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, Result<UserResponse>>
{
    private readonly IUser _currentUser;
    private readonly IIdentityService _identityService;

    public GetMeQueryHandler(IUser currentUser, IIdentityService identityService)
    {
        _currentUser = currentUser;
        _identityService = identityService;
    }

    public async Task<Result<UserResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.Id;

        if (string.IsNullOrEmpty(userId))
            return Result<UserResponse>.Failure(new[] { "Yetkisiz erişim." });

        // NOT: Profil bilgilerini doğrudan Handler içinde değil, IIdentityService üzerinden çekiyoruz.
        // Çünkü 'AppUser' (Identity) varlığı Infrastructure katmanına aittir ve Domain/Application 
        // katmanları bu somut sınıfa doğrudan bağımlı olmamalıdır. 
        // Bu yaklaşım, Identity mekanizması değişse bile Application katmanının etkilenmemesini sağlar.
        return await _identityService.GetUserProfileAsync(userId);
    }
}
