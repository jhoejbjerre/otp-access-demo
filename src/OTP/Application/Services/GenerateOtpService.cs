using Application.Commands;
using Application.Interfaces;
using Application.Options;
using Application.Utilities;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace Application.Services;

/// <summary>
///     Application service responsible for generating and storing one-time passwords (OTPs).
/// </summary>
public class GenerateOtpService : IGenerateOtpService
{
    private readonly IOtpRequestRepository _repository;
    private readonly OtpOptions _options;

    public GenerateOtpService(IOptions<OtpOptions> options, IOtpRequestRepository repository)
    {
        _options = options.Value;
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<string> GenerateOtpAsync(GenerateOtpCommand command, CancellationToken cancellationToken)
    {
        var otp = Random.Shared.Next(100000, 999999).ToString();
        var salt = _options.OtpSecretKey;
        var hashedOtp = OtpHasher.HashOtpWithSalt(otp, salt);

        var otpRequest = new OtpRequest
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            Phone = command.Phone,
            OtpCode = hashedOtp,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false,
            Created = DateTimeOffset.UtcNow
        };

        await _repository.AddAsync(otpRequest, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return otp;
    }
}