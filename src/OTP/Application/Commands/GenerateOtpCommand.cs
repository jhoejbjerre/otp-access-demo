namespace Application.Commands;

/// <summary>
///     Command representing the input required to request an OTP.
/// </summary>
public sealed record GenerateOtpCommand(
    string Email,
    string? Phone);
