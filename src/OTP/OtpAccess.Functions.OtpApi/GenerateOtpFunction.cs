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
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            GenerateOtpRequest dto = JsonConvert.DeserializeObject<GenerateOtpRequest>(requestBody);

            if (dto is null || string.IsNullOrWhiteSpace(dto.Email))
            {
                logger.LogError("Missing or invalid Email.");
                HttpResponseData badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Missing or invalid Email.");
                return badRequest;
            }

            Application.Commands.GenerateOtpCommand command = generateOtpCommandFactory.Create(dto);
            string otp = await otpService.GenerateOtpAsync(command);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"OTP generated for {command.Email}: {otp}");
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred while generating OTP.");
            HttpResponseData errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("An unexpected error occurred.");
            return errorResponse;
        }
    }
}
