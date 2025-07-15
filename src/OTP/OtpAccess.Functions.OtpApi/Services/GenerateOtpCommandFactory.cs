using Application.Commands;
using OtpAccess.Functions.OtpApi.Models.Requests;

namespace OtpAccess.Functions.OtpApi.Services;

public class GenerateOtpCommandFactory : IGenerateOtpCommandFactory
{
    public GenerateOtpCommand Create(GenerateOtpRequest request) =>
        new(
            request.Email,
            request.Phone);
}