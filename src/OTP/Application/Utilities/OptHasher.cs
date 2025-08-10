using System.Security.Cryptography;
using System.Text;

namespace Application.Utilities;

/// <summary>
///     Utility class for hashing OTP codes with a salt.
/// </summary>
public static class OtpHasher
{
    /// <summary>
    ///     Computes a SHA256 hash of the OTP combined with a salt.
    /// </summary>
    /// <param name="otp">The plain OTP.</param>
    /// <param name="salt">The salt value for hashing.</param>
    /// <returns>The hashed OTP as a hex string.</returns>
    public static string HashOtpWithSalt(string otp, string salt)
    {
        var combined = $"{otp}{salt}";
        var bytes = Encoding.UTF8.GetBytes(combined);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}