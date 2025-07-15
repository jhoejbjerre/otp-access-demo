using Application.Commands;
using Application.Common.Interfaces;
using Application.Options;
using Application.Utilities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class ValidateOtpService : IValidateOtpService
{
    private readonly IOtpRequestRepository _repository;
    private readonly OtpOptions _options;

    public ValidateOtpService(
        IOptions<OtpOptions> options,
        IOtpRequestRepository repository)
    {
        _options = options.Value;
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<bool> ValidateOtpAsync(ValidateOtpCommand command, CancellationToken cancellationToken = default)
    {
        var salt = _options.OtpSecretKey;
        var hashedOtp = OtpHasher.HashOtpWithSalt(command.OtpCode, salt);

        var success = await _repository.MarkAsUsedAsync(command.Email, hashedOtp, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return success;
    }
}