using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OtpValidate.Functions.OtpApi.Models.Requests;
using OtpValidate.Functions.OtpApi.Services;

namespace OtpValidate.Functions.OtpApi;

public abstract class ValidateOtpFunction(
    IValidateOtpService validateOtpService,
    IValidateOtpCommandFactory validateOtpCommandFactory,
    ILogger<ValidateOtpFunction> logger)
{
    [Function("ValidateOtp")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        FunctionContext executionContext)
    {
        logger.LogInformation("ValidateOtpFunction triggered.");

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var dto = JsonConvert.DeserializeObject<ValidateOtpRequest>(requestBody);

            if (dto is null || string.IsNullOrWhiteSpace(dto.Email))
            {
                logger.LogError("Missing or invalid Email.");
                var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Missing or invalid Email.");
                return badRequest;
            }

            var command = validateOtpCommandFactory.Create(dto);
            var otp = await validateOtpService.ValidateOtpAsync(command);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"OTP Validated for {command.Email}: {otp}");
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
