using Microsoft.AspNetCore.Identity;
using Noname.Domain.Entities;

namespace Noname.Infrastructure.Identity;

public class AppUser : IdentityUser
{
    public Member? Member { get; set; }
}