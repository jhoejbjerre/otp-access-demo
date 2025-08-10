namespace Application.Commands;

/// <summary>
///     Command representing the input required to validate an OTP.
/// </summary>
public sealed record ValidateOtpCommand(string Email, string OtpCode);
