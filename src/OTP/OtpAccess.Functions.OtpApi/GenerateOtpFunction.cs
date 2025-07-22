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

public sealed class GenerateOtpFunction(
    IGenerateOtpService otpService,
    IGenerateOtpCommandFactory generateOtpCommandFactory,
    ILogger<GenerateOtpFunction> logger)
{
    [Function("GenerateOtp")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        logger.LogInformation("GenerateOtpFunction triggered.");

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var dto = JsonConvert.DeserializeObject<GenerateOtpRequest>(requestBody);

            if (dto is null || string.IsNullOrWhiteSpace(dto.Email))
            {
                logger.LogError("Missing or invalid Email.");
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Missing or invalid Email.");
                return badRequest;
            }

            var command = generateOtpCommandFactory.Create(dto);
            var otp = await otpService.GenerateOtpAsync(command);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"OTP generated for {command.Email}: {otp}");
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred while generating OTP.");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("An unexpected error occurred.");
            return errorResponse;
        }
    }
}
