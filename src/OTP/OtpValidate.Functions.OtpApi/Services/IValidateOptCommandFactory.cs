using Application.Commands;

using OtpValidate.Functions.OtpApi.Models.Requests;

namespace OtpValidate.Functions.OtpApi.Services;

/// <summary>
///     Factory for creating ValidateOtpCommand instances from request models.
/// </summary>
public interface IValidateOtpCommandFactory
{
    ValidateOtpCommand Create(ValidateOtpRequest request);
}
