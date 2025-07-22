using System.Security.Claims;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace OtpAccess.Functions.OtpApi.Tests.Helpers;

/// <summary>
///     A simple fake implementation of <see cref="HttpRequestData" /> for use in unit testing Azure Functions.
///     This class allows you to simulate HTTP requests with a custom body payload, without requiring a real HTTP context.
///     The request body is provided as a string and exposed via the <see cref="Body" /> stream.
///     Other HTTP-related properties such as headers, cookies, method, and URL are initialized with safe defaults suitable
///     for testing.
/// </summary>
public sealed class FakeHttpRequestData : HttpRequestData
{
    private readonly MemoryStream _bodyStream;

    public FakeHttpRequestData(FunctionContext functionContext, string body)
        : base(functionContext) =>
        _bodyStream = new MemoryStream(Encoding.UTF8.GetBytes(body));

    public override Stream Body => _bodyStream;

    public override HttpHeadersCollection Headers { get; } = new();

    public override IReadOnlyCollection<IHttpCookie> Cookies => Array.Empty<IHttpCookie>();

    public override Uri Url => new("https://localhost");

    public override IEnumerable<ClaimsIdentity> Identities => Array.Empty<ClaimsIdentity>();

    public override string Method => "POST";

    public override HttpResponseData CreateResponse() => new FakeHttpResponseData(this);
}