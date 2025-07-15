using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OtpAccess.Functions.OtpApi.Models.Requests;
using OtpAccess.Functions.OtpApi.Services;

namespace OtpAccess.Functions.OtpApi;

public class GenerateOtpFunction
{
    private readonly IGenerateOtpCommandFactory _generateOtpCommandFactory;
    private readonly ILogger<GenerateOtpFunction> _logger;
    private readonly IGenerateOtpService _otpService;

    public GenerateOtpFunction(
        IGenerateOtpService otpService,
        IGenerateOtpCommandFactory generateOtpCommandFactory,
        ILogger<GenerateOtpFunction> logger)
    {
        _logger = logger;
        _otpService = otpService;
        _generateOtpCommandFactory = generateOtpCommandFactory;
    }

    [Function("GenerateOtp")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        _logger.LogInformation("GenerateOtpFunction triggered.");

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var dto = JsonConvert.DeserializeObject<GenerateOtpRequest>(requestBody);

            if (dto is null || string.IsNullOrWhiteSpace(dto.Email))
            {
                _logger.LogError("Missing or invalid Email.");
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Missing or invalid Email.");
                return badRequest;
            }

            var command = _generateOtpCommandFactory.Create(dto);
            var otp = await _otpService.GenerateOtpAsync(command);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"OTP generated for {command.Email}: {otp}");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred while generating OTP.");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("An unexpected error occurred.");
            return errorResponse;
        }
    }
}
