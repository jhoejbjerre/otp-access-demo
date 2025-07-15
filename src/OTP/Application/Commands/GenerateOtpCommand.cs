namespace Application.Commands;

public sealed record GenerateOtpCommand(
    string Email,
    string? Phone);