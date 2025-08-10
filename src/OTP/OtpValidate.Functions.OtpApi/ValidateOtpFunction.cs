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
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ValidateOtpRequest dto = JsonConvert.DeserializeObject<ValidateOtpRequest>(requestBody);

            if (dto is null || string.IsNullOrWhiteSpace(dto.Email))
            {
                logger.LogError("Missing or invalid Email.");
                HttpResponseData badRequest = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequest.WriteStringAsync("Missing or invalid Email.");
                return badRequest;
            }

            Application.Commands.ValidateOtpCommand command = validateOtpCommandFactory.Create(dto);
            bool otp = await validateOtpService.ValidateOtpAsync(command);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"OTP Validated for {command.Email}: {otp}");
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
