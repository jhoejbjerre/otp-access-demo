using Application.Commands;

namespace Application.Interfaces;

/// <summary>
///     Abstraction for application service responsible for generating and storing one-time passwords (OTPs).
/// </summary>
public interface IGenerateOtpService
{
    /// <summary>
    ///     Generates a 6-digit one-time password, hashes it with a salt, and stores it in the database.
    /// </summary>
    /// <param name="command">The input data for generating the OTP.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The plain-text OTP for user delivery.</returns>
    Task<string> GenerateOtpAsync(GenerateOtpCommand command, CancellationToken cancellationToken = default);
}
