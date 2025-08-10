using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;

namespace OtpAccess.Functions.OtpApi.Tests.Helpers;

/// <summary>
///     A simple fake implementation of <see cref="FunctionContext" /> for use in unit testing Azure Functions.
///     This fake allows you to create <see cref="HttpRequestData" /> and <see cref="HttpResponseData" />
///     without relying on the real Azure Functions runtime.
///     This implementation provides only the minimal members required for testing and leaves other members null.
/// </summary>

public sealed class FakeFunctionContext : FunctionContext
{
    public override string InvocationId => Guid.NewGuid().ToString();
    public override string FunctionId => Guid.NewGuid().ToString();
    public override TraceContext TraceContext => null;
    public override BindingContext BindingContext => null;
    public override FunctionDefinition FunctionDefinition => null;
    public override IServiceProvider InstanceServices { get; set; } = new ServiceCollection().BuildServiceProvider();
    public override IDictionary<object, object> Items { get; set; } = new Dictionary<object, object>();

    public override IInvocationFeatures Features => null;

    /// <summary>
    ///     Mock implementation of RetryContext. Null unless your function uses retries.
    /// </summary>
    public override RetryContext RetryContext => null;
}
