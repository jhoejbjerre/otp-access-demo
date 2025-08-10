using Application.Commands;
using Application.Options;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Moq;

namespace OtpAccess.Functions.OtpApi.Tests;

/// <summary>
///     Unit tests for <see cref="GenerateOtpService" />.
/// </summary>
public sealed class GenerateOtpServiceTests
{
    private readonly Mock<IOtpRequestRepository> _repositoryMock;
    private readonly GenerateOtpService _service;

    public GenerateOtpServiceTests()
    {
        _repositoryMock = new Mock<IOtpRequestRepository>();
        var options = Options.Create(new OtpOptions { OtpSecretKey = "fake-secret" });
        _service = new GenerateOtpService(options, _repositoryMock.Object);
    }

    [Fact]
    public async Task GenerateOtpAsync_Returns6DigitOtp_AndStoresRequest()
    {
        // Arrange
        var command = new GenerateOtpCommand("test@example.com", "+4512345678");
        OtpRequest capturedRequest = null;

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<OtpRequest>(), It.IsAny<CancellationToken>()))
            .Callback<OtpRequest, CancellationToken>((r, _) => capturedRequest = r)
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var otp = await _service.GenerateOtpAsync(command, CancellationToken.None);

        // Assert
        Assert.Equal(6, otp.Length);
        Assert.True(int.TryParse(otp, out _));

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<OtpRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.NotNull(capturedRequest);
        Assert.Equal(command.Email, capturedRequest.Email);
        Assert.Equal(command.Phone, capturedRequest.Phone);
        Assert.False(capturedRequest.IsUsed);
        Assert.True(capturedRequest.ExpiresAt > DateTime.UtcNow);
        Assert.True(capturedRequest.ExpiresAt < DateTime.UtcNow.AddMinutes(11));
    }
}
