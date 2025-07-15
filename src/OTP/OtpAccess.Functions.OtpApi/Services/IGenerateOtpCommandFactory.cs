using Application.Commands;
using OtpAccess.Functions.OtpApi.Models.Requests;

namespace OtpAccess.Functions.OtpApi.Services;

public interface IGenerateOtpCommandFactory
{
    GenerateOtpCommand Create(GenerateOtpRequest request);
}