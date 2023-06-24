using Jinget.AzureDevOps.RestApi.Connector.Board.Tests;
using Jinget.AzureDevOps.RestApi.Connector.Teams;
using Jinget.AzureDevOps.RestApi.Connector.Teams.ViewModels;

namespace Jinget.AzureDevOps.RestApi.ConnectorTests.TeamsTests
{
    [TestClass()]
    public class GeneralConnectorTests : _BaseTests
    {
        TeamConnector connector;

        [TestInitialize]
        public void TestInitialize() => connector = new TeamConnector(pat, organization);

        [TestMethod()]
        public async Task should_get_list_of_all_teams()
        {
            TeamListViewModel result = await connector.GetAllTeams();

            Assert.IsTrue(result.count > 0);
        }

        [TestMethod()]
        public async Task should_get_list_of_teams_in_specific_project()
        {
            TeamListViewModel result = await connector.GetTeams(Guid.Parse("5ec32e71-d47d-4908-8107-069a952c9550"));

            Assert.IsTrue(result.count > 0);
        }

        [TestMethod()]
        public async Task should_get_specific_project_team()
        {
            TeamViewModel result = await connector.Get(Guid.Parse("5ec32e71-d47d-4908-8107-069a952c9550"), "PMOSample Team");

            Assert.IsTrue(result.id != "");
        }
    }
}