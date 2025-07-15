using Application.Commands;

namespace Application.Common.Interfaces;

/// <summary>
///     Abstraction for validating and consuming one-time passwords (OTPs).
/// </summary>
public interface IValidateOtpService
{
    /// <summary>
    ///     Validates an OTP by email and OTP code to see is this is a valid password in a given login session.
    ///     If valid, the OTP is marked as used.
    /// </summary>
    /// <param name="command">The command containing the email address and one-time password (OTP) to validate.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>True if the OTP was valid and successfully marked as used; otherwise, false.</returns>
    Task<bool> ValidateOtpAsync(ValidateOtpCommand command, CancellationToken cancellationToken = default);
}