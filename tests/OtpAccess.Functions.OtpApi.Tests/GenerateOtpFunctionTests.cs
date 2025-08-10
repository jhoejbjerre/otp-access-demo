using System.Net;

using Application.Commands;
using Application.Interfaces;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

using Moq;

using OtpAccess.Functions.OtpApi.Models.Requests;
using OtpAccess.Functions.OtpApi.Services;
using OtpAccess.Functions.OtpApi.Tests.Helpers;

namespace OtpAccess.Functions.OtpApi.Tests;


public sealed class GenerateOtpFunctionTests
{
    private readonly Mock<IGenerateOtpService> _otpServiceMock = new();
    private readonly Mock<IGenerateOtpCommandFactory> _commandFactoryMock = new();
    private readonly Mock<ILogger<GenerateOtpFunction>> _loggerMock = new();
    private readonly FunctionContext _functionContext = new FakeFunctionContext();

    [Fact]
    public async Task Run_ReturnsBadRequest_WhenEmailIsMissing()
    {
        var function = new GenerateOtpFunction(_otpServiceMock.Object, _commandFactoryMock.Object, _loggerMock.Object);
        string requestBody = /*lang=json,strict*/ @"{ ""phone"": ""+4512345678"" }";

        var req = new FakeHttpRequestData(_functionContext, requestBody);

        Microsoft.Azure.Functions.Worker.Http.HttpResponseData response = await function.Run(req, _functionContext);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Run_ReturnsOk_WhenEmailIsValid()
    {
        _ = _otpServiceMock
            .Setup(s => s.GenerateOtpAsync(It.IsAny<GenerateOtpCommand>(), CancellationToken.None))
            .ReturnsAsync("123456");

        _ = _commandFactoryMock
            .Setup(f => f.Create(It.IsAny<GenerateOtpRequest>()))
            .Returns(new GenerateOtpCommand("test@example.com", "+4512345678"));

        var function = new GenerateOtpFunction(_otpServiceMock.Object, _commandFactoryMock.Object, _loggerMock.Object);
        string requestBody = /*lang=json,strict*/ @"{ ""email"": ""test@example.com"", ""phone"": ""+4512345678"" }";

        var req = new FakeHttpRequestData(_functionContext, requestBody);

        Microsoft.Azure.Functions.Worker.Http.HttpResponseData response = await function.Run(req, _functionContext);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
