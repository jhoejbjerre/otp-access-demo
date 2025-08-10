using Domain.Common;

namespace Domain.Entities;

public sealed class OtpRequest : BaseAuditableEntity
{
    public string Email { get; init; } = default!;
    public string? Phone { get; init; }
    public string OtpCode { get; init; } = default!;
    public DateTime ExpiresAt { get; init; }
    public bool IsUsed { get; set; }
}
