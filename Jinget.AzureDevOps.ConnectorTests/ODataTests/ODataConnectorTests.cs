using Jinget.AzureDevOps.Connector.OData;
using Jinget.AzureDevOps.ConnectorTests.ODataTests.CustomTypes;

namespace Jinget.AzureDevOps.ConnectorTests.ODataTests;

[TestClass()]
public class ODataConnectorTests : BaseTests
{
    ODataConnector connector;

    [TestInitialize]
    public void TestInitialize()
        => connector = new ODataConnector(ServiceProvider, pat, baseUrl, organization, project: projectName);

    [TestMethod()]
    public async Task should_get_workitems_from_one_project()
    {
        var queries = new Dictionary<string, string>()
        {
            {"$filter","WorkItemType eq 'Task' and State eq 'To Do'"},
            {"$select","WorkItemId,Title,AssignedTo,State" }
        };

        var result = await connector.QueryAsync("WorkItems", queries);
        Assert.IsNotNull(result?.odatacontext);
    }

    [TestMethod()]
    public async Task should_get_workitems_from_one_project_and_deserial_it_to_custom_type()
    {
        var queries = new Dictionary<string, string>()
        {
            {"$filter","WorkItemType eq 'Task' and State eq 'To Do'"},
            {"$select","WorkItemId,Title,AssignedTo,State" }
        };

        var result = await connector.QueryAsync<MyCustomType>("WorkItems", queries);
        Assert.IsNotNull(result?.odatacontext);
        Assert.AreNotEqual("", result.value[0].Title);
    }

    [TestMethod()]
    public async Task should_get_workitems_from_multiple_projects()
    {
        var otherConnector = new ODataConnector(ServiceProvider, pat, "https://analytics.dev.azure.com", organization);

        var queries = new Dictionary<string, string>()
        {
            {"$filter",$"(Project/ProjectName eq '{projectName}' or Project/ProjectName eq 'On.NETLive') and WorkItemType eq 'Task' and State eq 'To Do'"},
            {"$select","WorkItemId,Title,AssignedTo,State" }
        };

        var result = await otherConnector.QueryAsync("WorkItems", queries);
        Assert.IsNotNull(result?.odatacontext);
    }
}
