using Jinget.AzureDevOps.Connector.Teams;

namespace Jinget.AzureDevOps.ConnectorTests.TeamsTests;

[TestClass()]
public class TeamConnectorTests : BaseTests
{
    TeamConnector connector;

    [TestInitialize]
    public void TestInitialize() => connector = new TeamConnector(ServiceProvider, pat: pat, organization: organization, apiVersion: "7.1-preview.3");

    [TestMethod()]
    public async Task should_get_list_of_all_teams()
    {
        var result = await connector.GetAllTeamsAsync();
        Assert.IsNotNull(result);
        Assert.IsGreaterThan(0, result.count);
    }

    [TestMethod()]
    public async Task should_get_list_of_teams_in_specific_project()
    {
        var result = await connector.GetTeamsAsync(Guid.Parse(projectId));
        Assert.IsNotNull(result);
        Assert.IsGreaterThan(0, result.count);
    }

    [TestMethod()]
    public async Task should_get_specific_project_team()
    {
        var result = await connector.GetAsync(Guid.Parse(projectId), teamName);
        Assert.AreNotEqual("", result?.id);
    }
}