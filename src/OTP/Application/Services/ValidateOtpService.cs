using Application.Interfaces;
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
    public async Task<bool> ValidateOtpAsync(string email, string otp, CancellationToken cancellationToken = default)
    {
        var salt = _options.OtpSecretKey;
        var hashedOtp = OtpHasher.HashOtpWithSalt(otp, salt);

        var success = await _repository.MarkAsUsedAsync(email, hashedOtp, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return success;
    }
}