using Application.Commands;

using OtpValidate.Functions.OtpApi.Models.Requests;

namespace OtpValidate.Functions.OtpApi.Services;

/// <inheritdoc />
public class ValidateOtpCommandFactory : IValidateOtpCommandFactory
{
    public ValidateOtpCommand Create(ValidateOtpRequest request) =>
        new(
            request.Email,
            request.OtpCode);
}
