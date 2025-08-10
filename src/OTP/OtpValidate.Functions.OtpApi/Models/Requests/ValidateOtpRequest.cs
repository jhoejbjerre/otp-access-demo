namespace OtpValidate.Functions.OtpApi.Models.Requests;

public sealed record ValidateOtpRequest(
    string Email,
    string OtpCode);
