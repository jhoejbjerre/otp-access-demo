using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

namespace OtpAccess.Functions.OtpApi.Tests.Helpers;

/// <summary>
///     A simple fake implementation of <see cref="HttpResponseData" /> for use in unit testing Azure Functions.
///     This class provides a writable in-memory response body stream, allowing inspection of the HTTP response content
///     after function execution.
///     Default values are provided for headers, status code, and cookies to simplify test setup.
///     Use <see cref="GetBodyAsString" /> to retrieve the written response body as a string for assertions in tests.
/// </summary>
public sealed class FakeHttpResponseData(HttpRequestData request) : HttpResponseData(request.FunctionContext)
{
    private MemoryStream _bodyStream = new();

    public override Stream Body
    {
        get => _bodyStream;
        set => _bodyStream = (MemoryStream)value;
    }

    public override HttpHeadersCollection Headers { get; set; } = [];

    public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    public override HttpCookies Cookies => null;

    public string GetBodyAsString()
    {
        Body.Position = 0;
        using var reader = new StreamReader(Body);
        return reader.ReadToEnd();
    }
}