using Noname.Domain.Enums;

namespace Noname.Domain.Common;

public abstract class BaseEntity : ISoftDelete
{
    public Guid Id { get; set; }
    public long CreatedAt { get; set; }
    public long? UpdatedAt { get; set; }
    public string? CreatedById { get; set; }
    public string? UpdatedById { get; set; }
    public bool IsDeleted { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.Draft;
}
