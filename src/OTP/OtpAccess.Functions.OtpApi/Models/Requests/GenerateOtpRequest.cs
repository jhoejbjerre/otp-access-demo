namespace OtpAccess.Functions.OtpApi.Models.Requests;

public sealed record GenerateOtpRequest(
    string Email,
    string? Phone);
