using Jinget.Handlers.ExternalServiceHandlers.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Jinget.AzureDevOps.ConnectorTests;

public class BaseTests
{
    protected const string baseUrl = "https://analytics.dev.azure.com";

    protected const string pat = "...";
    protected const string organization = "farahmandian";

    protected const string projectName = "MSFarsi";
    protected const string projectId = "5dc5c967-b5a6-4c33-9d2d-679857d66b0a";
    protected const string teamName = "MSFarsi Team";

    protected const string basicProcessId = "b8a3a935-7e91-48b8-a94c-606d37c3e9f2";
    protected const string cmmiProcessId = "27450541-8e31-4150-9947-dc59f998fc01";

    protected IServiceProvider ServiceProvider { get; }
    public BaseTests()
    {
        var services = new ServiceCollection();
        services.AddJingetExternalServiceHandler("jinget-client", true);
        ServiceProvider = services.BuildServiceProvider();
    }
}