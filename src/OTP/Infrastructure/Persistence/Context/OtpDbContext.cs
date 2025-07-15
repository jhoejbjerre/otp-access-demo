using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context;

public class OtpDbContext : DbContext
{
    public OtpDbContext(DbContextOptions<OtpDbContext> options) : base(options)
    {
    }

    public DbSet<OtpRequest> OtpRequests => Set<OtpRequest>();

    protected override void OnModelCreating(ModelBuilder builder) => ConfigureOtpRequestEntity(builder);

    private static void ConfigureOtpRequestEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<OtpRequest>();

        entity.ToTable("OtpRequest", "dto");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Email)
              .IsRequired()
              .HasMaxLength(200);

        entity.Property(e => e.Phone)
              .HasMaxLength(20);

        entity.Property(e => e.OtpCode)
              .IsRequired()
              .HasMaxLength(64);

        entity.Property(e => e.ExpiresAt)
              .IsRequired();

        entity.Property(e => e.IsUsed)
              .IsRequired()
              .HasDefaultValue(false);

        entity.Property(e => e.Created)
              .IsRequired()
              .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        entity.Property(e => e.LastModified)
              .HasDefaultValueSql("SYSDATETIMEOFFSET()");
    }
}
