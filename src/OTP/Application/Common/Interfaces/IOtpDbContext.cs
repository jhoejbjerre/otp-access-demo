using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IOtpDbContext
{
    DbSet<OtpRequest> OtpRequests { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}