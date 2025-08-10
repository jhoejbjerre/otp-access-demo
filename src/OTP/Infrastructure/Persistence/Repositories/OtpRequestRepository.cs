using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
///     Repository for handling persistence operations on <see cref="OtpRequest" /> entities.
///     Provides methods for querying and updating OTP requests in the database.
/// </summary>
/// <param name="dbContext">The <see cref="OtpDbContext" /> used for data access.</param>
/// <param name="logger">The <see cref="ILogger{OtpRequestRepository}" /> used for diagnostic logging.</param>
/// <remarks>
///     This level of logging is intentionally excessive and is not representative of production best practices.
///     It is included solely to demonstrate that database-related operations are observable through logging,
///     such as viewing activity in Application Insights during development and testing.
/// </remarks>
/// <remarks>
/// </remarks>
/// <param name="dbContext"></param>
/// <param name="logger"></param>
public sealed class OtpRequestRepository(
    OtpDbContext dbContext,
    ILogger<OtpRequestRepository> logger) : IOtpRequestRepository
{
    private readonly OtpDbContext _dbContext = dbContext;
    private readonly ILogger<OtpRequestRepository> _logger = logger;

    /// <inheritdoc />
    public async Task AddAsync(OtpRequest entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.OtpRequests.AddAsync(entity, cancellationToken);
        _logger.LogInformation("Added new OTP request with Id '{Id}' for email '{Email}'.", entity.Id, entity.Email);
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Saving changes to database...");
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogDebug("Changes saved successfully.");
    }

    /// <inheritdoc />
    public async Task<bool> MarkAsUsedAsync(string email, string hashedOtp, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Attempting to mark OTP as used for email '{Email}'.", email);

        var otpRequest = await _dbContext.OtpRequests
                                         .FirstOrDefaultAsync(
                                             x => x.Email == email && x.OtpCode == hashedOtp && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow,
                                             cancellationToken);

        if (otpRequest == null)
        {
            _logger.LogWarning("No valid OTP found to mark as used for email '{Email}'.", email);
            return false;
        }

        otpRequest.IsUsed = true;
        otpRequest.LastModified = DateTimeOffset.UtcNow;

        _logger.LogInformation("OTP with Id '{Id}' for email '{Email}' marked as used.", otpRequest.Id, otpRequest.Email);
        return true;
    }
}