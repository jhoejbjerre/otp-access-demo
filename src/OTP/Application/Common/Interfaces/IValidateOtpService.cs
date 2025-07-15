namespace Application.Interfaces;

/// <summary>
///     Abstraction for validating and consuming one-time passwords (OTPs).
/// </summary>
public interface IValidateOtpService
{
    /// <summary>
    ///     Validates an OTP by email and raw code.
    ///     If valid, the OTP is marked as used.
    /// </summary>
    /// <param name="email">The email address associated with the OTP.</param>
    /// <param name="otp">The plain-text OTP submitted by the user.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>True if the OTP was valid and successfully marked as used; otherwise, false.</returns>
    Task<bool> ValidateOtpAsync(string email, string otp, CancellationToken cancellationToken = default);
}