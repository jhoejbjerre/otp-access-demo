using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
///     Abstraction for OTP request persistence operations.
///     Defines methods for querying and updating <see cref="OtpRequest" /> entities.
/// </summary>
public interface IOtpRequestRepository
{
    /// <summary>
    ///     Adds a new OTP request to the persistence store.
    /// </summary>
    /// <param name="entity">The <see cref="OtpRequest" /> to add.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    Task AddAsync(OtpRequest entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Marks an OTP as used by matching email and hashed OTP.
    /// </summary>
    /// <param name="email">The email address associated with the OTP request.</param>
    /// <param name="hashedOtp">The hashed OTP value.</param>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    /// <returns>True if an OTP was found and marked as used; otherwise, false.</returns>
    Task<bool> MarkAsUsedAsync(string email, string hashedOtp, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Commits all pending changes to the persistence store.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for async operation.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
