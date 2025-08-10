using Application.Commands;
using Application.Common.Interfaces;
using Application.Options;
using Application.Utilities;

using Domain.Interfaces;

using Microsoft.Extensions.Options;

namespace Application.Services;

public sealed class ValidateOtpService(
    IOptions<OtpOptions> options,
    IOtpRequestRepository repository) : IValidateOtpService
{
    private readonly OtpOptions _options = options.Value;

    /// <inheritdoc />
    public async Task<bool> ValidateOtpAsync(ValidateOtpCommand command, CancellationToken cancellationToken = default)
    {
        string salt = _options.OtpSecretKey;
        string hashedOtp = OtpHasher.HashOtpWithSalt(command.OtpCode, salt);

        bool success = await repository.MarkAsUsedAsync(command.Email, hashedOtp, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return success;
    }
}
