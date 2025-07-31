using Jinget.AzureDevOps.Connector.Processes;

namespace Jinget.AzureDevOps.ConnectorTests.ProcessTests;

[TestClass()]
public class ProcessTests : BaseTests
{
    ProcessConnector connector;

    [TestInitialize]
    public void TestInitialize()
        => connector = new ProcessConnector(ServiceProvider, pat, organization);

    [TestMethod()]
    public async Task should_get_list_of_processes()
    {
        var result = await connector.ListAsync();
        Assert.IsNotNull(result);
        Assert.IsGreaterThan(0, result.count);
    }

    [TestMethod()]
    public async Task should_get_specific_process_detail()
    {
        var result = await connector.GetAsync(Guid.Parse(cmmiProcessId));
        Assert.AreNotEqual("", result?.id);
    }
}