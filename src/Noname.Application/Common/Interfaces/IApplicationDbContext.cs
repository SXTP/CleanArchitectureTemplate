using Microsoft.EntityFrameworkCore;
using Noname.Domain.Entities;

namespace Noname.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Member> Members { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}