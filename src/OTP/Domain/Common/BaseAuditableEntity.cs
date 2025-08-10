namespace Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTimeOffset Created { get; init; }

    public DateTimeOffset? LastModified { get; set; }
}
