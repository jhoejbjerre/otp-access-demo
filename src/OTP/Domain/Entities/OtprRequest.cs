using Domain.Common;

namespace Domain.Entities;

public class OtpRequest : BaseAuditableEntity
{
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string OtpCode { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
}
